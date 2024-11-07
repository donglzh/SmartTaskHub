using Confluent.Kafka;

namespace SmartTaskHub.Infrastructure.Kafka;

/// <summary>
/// kafka 生产者实现
/// </summary>
public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(string bootstrapServers)
    {
        var config = new ProducerConfig{ BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="topic">主题</param>
    /// <param name="message">消息</param>
    public async Task SendMessageAsync(string topic, string message)
    {
        try
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        }
        catch (ProduceException<Null,string> ex)
        {
            throw new Exception($"发送失败, 错误: {ex.Error.Reason}");
        }
    }
}