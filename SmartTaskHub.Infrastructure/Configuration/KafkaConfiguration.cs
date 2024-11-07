namespace SmartTaskHub.Infrastructure.Configuration;

/// <summary>
/// Kafka 配置项
/// </summary>
public class KafkaConfiguration
{
    /// <summary>
    /// BootstrapServers
    /// </summary>
    public string BootstrapServers { get; set; } = string.Empty;
    
    /// <summary>
    /// GroupId
    /// </summary>
    public string GroupId { get; set; } = string.Empty;
    
    /// <summary>
    /// Topic
    /// </summary>
    public string Topic { get; set; } = string.Empty;
}