using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.Poller;

public class TaskPoller
{
    private readonly ILogger<TaskPoller> _logger;
    private readonly ITaskRegistrationService _taskRegistrationService;
    private readonly IKafkaService _kafkaService;
    private readonly string _topic;
    private readonly int _pollingIntervalInMilliseconds;
    
    public TaskPoller(ILogger<TaskPoller> logger,ITaskRegistrationService taskRegistrationService,
        IKafkaService kafkaService,IConfiguration configuration)
    {
        _logger = logger;
        _taskRegistrationService = taskRegistrationService;
        _kafkaService = kafkaService;
        _pollingIntervalInMilliseconds = GetPollingInterval(configuration);
        _topic = GetTopic(configuration);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await CheckForOverdueTasksAsync();
            
            // 应用暂停执行，_pollingIntervalInMilliseconds 毫秒后，继续执行
            await Task.Delay(_pollingIntervalInMilliseconds, cancellationToken);
        }
    }

    /// <summary>
    /// 检查过期的任务
    /// </summary>
    private async Task CheckForOverdueTasksAsync()
    {
        var overdueTasks = await _taskRegistrationService.GetOverdueTasksAsync();
        foreach (var key in overdueTasks)
        {
            try
            {
                await ProcessTaskAsync(key);
                await _taskRegistrationService.RemoveFromQueueAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{key} 任务处理错误");
            }
        }
    }
    
    /// <summary>
    /// 发布 kafka
    /// </summary>
    /// <param name="key"></param>
    private async Task ProcessTaskAsync(string key)
    {
        await _kafkaService.SendMessageAsync(_topic, key);
        _logger.LogInformation($"{key} 任务已处理并将任务信息发送到 kafka {_topic} 主题中");
        
    }
    
    private int GetPollingInterval(IConfiguration configuration)
    {
        return configuration.GetValue<int>("Polling:PollingIntervalInMinutes") * 60 * 1000;
    }

    private string GetTopic(IConfiguration configuration)
    {
        return configuration.GetValue<string>("Kafka:Topic") ?? _topic;
    }
}