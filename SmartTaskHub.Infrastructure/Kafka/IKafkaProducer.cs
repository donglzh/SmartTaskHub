namespace SmartTaskHub.Infrastructure.Kafka;

/// <summary>
/// Kafka 生产者接口
/// </summary>
public interface IKafkaProducer
{
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="topic">主题</param>
    /// <param name="message">消息</param>
    /// <returns></returns>
    Task SendMessageAsync(string topic, string message);
}