using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartTaskHub.Infrastructure.Redis;
using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.Consumer;

public class KafkaConsumer
{
    private readonly ILogger<KafkaConsumer> _logger;
    private readonly ITaskRegistrationService _taskRegistrationService;
    private readonly ITaskTimeoutRuleService _taskTimeoutRuleService;
    private readonly string _bootstrapServers;
    private readonly string _groupId;
    private readonly string _topic;
    private readonly ConsumerConfig _consumerConfig;

    public KafkaConsumer(ILogger<KafkaConsumer> logger, ITaskRegistrationService taskRegistrationService,
        ITaskTimeoutRuleService taskTimeoutRuleService,IConfiguration configuration)
    {
        _logger = logger;
        _taskRegistrationService = taskRegistrationService;
        _taskTimeoutRuleService = taskTimeoutRuleService;
        _bootstrapServers = GetBootstrapServers(configuration);
        _groupId = GetGroupId(configuration);
        _topic = GetTopic(configuration);
        
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            // Earliest 表示消费者在没有初始偏移量或偏移量越界时，从分区的起始位置（最早的消息）开始消费
            AutoOffsetReset = AutoOffsetReset.Earliest,
            // 关闭自动提交，使用手动提交偏移量
            EnableAutoCommit = false 
        };
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Null, string>(_consumerConfig).Build();
        consumer.Subscribe(_topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(cancellationToken);
                    await ProcessTaskAsync(cr.Message.Value);

                    // 手动提交偏移量
                    consumer.Commit(cr);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"消费消息时发送错误: {ex.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("消费操作已取消, 关闭消费者");
        }
        finally
        {
            // 清理
            consumer.Close(); 
        }
    }


    /// <summary>
    /// 处理
    /// </summary>
    /// <param name="key">key(task)</param>
    private async Task ProcessTaskAsync(string key)
    {
        try
        {
            var taskDetails = await _taskRegistrationService.GetTaskDetailsAsync(key);
            switch (taskDetails.Level)
            {
                case TaskLevel.Level1:
                    await SendWechatMessageAsync(key,"Level1");
                    await UpgradeAsync(key, taskDetails);
                    break;
                case TaskLevel.Level2:
                    await SendWechatMessageAsync(key,"Level2");
                    await _taskRegistrationService.DeleteTaskDetailsAsync(key);
                    break;
                default:
                    _logger.LogWarning($"{key} 任务等级未知");
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"{key} 任务处理错误");
        }
    }

    /// <summary>
    /// 升级
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">value</param>
    public async Task UpgradeAsync(string key,TaskDetailsDto value) 
    {
        // 原预计执行时间
        var originalDateTime = FromTimestamp(value.ScheduledTime);
        
        // 超时时长, 默认 10 分钟
        TimeSpan timeoutDuration = new TimeSpan(0, 0, 10, 0);
        
        // 根据 TaskType 获取系统内该 TaskType 的超时时长
        var taskTimeoutRule = await _taskTimeoutRuleService.GetTaskTimeoutRuleByTaskTypeAsync(value.TaskType);
        if (taskTimeoutRule != null)
        {
            timeoutDuration = taskTimeoutRule.TimeoutDuration;
        }
        
        var newScheduledTime = ToTimestamp(originalDateTime.Add(timeoutDuration));

        var request = new RegisterTaskRequest()
        {
            Key = key,
            Code = value.Code,
            TaskType = value.TaskType,
            Level = TaskLevel.Level2,
            ScheduledTime = newScheduledTime,
        };
        await _taskRegistrationService.RegisterTaskAsync(request);
        _logger.LogInformation($"{key} 任务已升级，下次提醒时间为：{newScheduledTime}");
    }
    
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="key">key(task)</param>
    /// <param name="level">level</param>
    private async Task SendWechatMessageAsync(string key,string level)
    {
        _logger.LogInformation($"{key} 任务超时提醒已发送至微信，等级：{level}");
        await Task.CompletedTask;
    }
    
    /// <summary>
    /// 获取 BootstrapServers 配置
    /// </summary>
    /// <param name="configuration">configuration</param>
    /// <returns></returns>
    private string GetBootstrapServers(IConfiguration configuration)
    {
        return configuration.GetValue<string>("Kafka:BootstrapServers") ?? _bootstrapServers;
    }

    /// <summary>
    /// 获取 GroupId 配置
    /// </summary>
    /// <param name="configuration">configuration</param>
    /// <returns></returns>
    private string GetGroupId(IConfiguration configuration)
    {
        return configuration.GetValue<string>("Kafka:GroupId") ?? _groupId;
    }
    
    /// <summary>
    /// 获取 Topic 配置
    /// </summary>
    /// <param name="configuration">configuration</param>
    /// <returns></returns>
    private string GetTopic(IConfiguration configuration)
    {
        return configuration.GetValue<string>("Kafka:Topic") ?? _topic;
    }
    
    /// <summary>
    /// DateTime 转换为时间戳（秒）
    /// </summary>
    /// <param name="dateTime">日期时间</param>
    /// <returns>时间戳</returns>
    public long ToTimestamp(DateTime dateTime)
    {
        // 确保 dateTime 是 UTC 时间
        if (dateTime.Kind == DateTimeKind.Unspecified)
        {
            // 如果未指定，可以假设是 UTC
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    
        // 如果是本地时间，则转换为 UTC
        if (dateTime.Kind == DateTimeKind.Local)
        {
            dateTime = dateTime.ToUniversalTime();
        }
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }
    
    /// <summary>
    /// 时间戳（秒）转换为 DateTime
    /// </summary>
    /// <param name="timestamp">时间戳（秒）</param>
    /// <returns>日期时间</returns>
    public static DateTime FromTimestamp(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
    }
}