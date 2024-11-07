using SmartTaskHub.Infrastructure.Kafka;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.Service;

/// <summary>
/// kafka 服务实现
/// </summary>
public class KafkaService : IKafkaService
{
    private readonly IKafkaProducer _kafkaProducer;
    
    public KafkaService(IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="topic">主题</param>
    /// <param name="message">消息</param>
    public async Task SendMessageAsync(string topic, string message)
    {
        if (string.IsNullOrEmpty(topic))
        {
            throw new Exception("主题不能为空或空值");
        }

        if (string.IsNullOrEmpty(message))
        {
            throw new Exception("消息不能为空或空值");
        }
        
        await _kafkaProducer.SendMessageAsync(topic, message);
    }
}