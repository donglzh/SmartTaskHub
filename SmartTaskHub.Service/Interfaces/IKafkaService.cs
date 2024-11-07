namespace SmartTaskHub.Service.Interfaces;

/// <summary>
/// kafka 服务接口
/// </summary>
public interface IKafkaService
{
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="topic">主题</param>
    /// <param name="message">消息</param>
    Task SendMessageAsync(string topic, string message);
}