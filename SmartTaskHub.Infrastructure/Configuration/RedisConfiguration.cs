namespace SmartTaskHub.Infrastructure.Configuration;

/// <summary>
/// Redis 配置项
/// </summary>
public class RedisConfiguration
{
    /// <summary>
    /// Redis 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
    
    /// <summary>
    /// 任务键的前缀（Redis字符串数据类型）
    /// </summary>
    public string TaskKeyPrefix { get; set; } = string.Empty;
    
    /// <summary>
    /// 任务计划键（Redis有序集合 sorted set）
    /// </summary>
    public string ScheduledTaskKey { get; set; } = string.Empty;
}