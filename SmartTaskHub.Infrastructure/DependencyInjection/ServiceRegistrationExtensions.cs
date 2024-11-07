using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartTaskHub.Infrastructure.Configuration;
using SmartTaskHub.Infrastructure.Kafka;
using SmartTaskHub.Infrastructure.Redis;
using SmartTaskHub.Infrastructure.Redis.Interface;
using SmartTaskHub.Infrastructure.Redis.Repositories;
using StackExchange.Redis;

namespace SmartTaskHub.Infrastructure.DependencyInjection;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 注册 Redis 配置
        services.Configure<RedisConfiguration>(configuration.GetSection("Redis"));
        services.AddSingleton(provider => 
            provider.GetRequiredService<IOptions<RedisConfiguration>>().Value);
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            var redisOptions = provider.GetRequiredService<IOptions<RedisConfiguration>>().Value;
            return ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
        });
        services.AddSingleton<RedisConnectionManager>(provider =>
        {
            var redisOptions = provider.GetRequiredService<IOptions<RedisConfiguration>>().Value;
            return new RedisConnectionManager(redisOptions.ConnectionString);
        });
        services.AddSingleton<IDatabase>(provider =>
        {
            var connectionMultiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
            return connectionMultiplexer.GetDatabase();
        });
        services.AddSingleton<IRedisSortedSet, TaskRegistrationRepository>();
        services.AddScoped<IRedisString, TaskRegistrationRepository>();

        // 注册 Kafka 配置
        services.Configure<KafkaConfiguration>(configuration.GetSection("Kafka"));
        services.AddSingleton<IKafkaProducer, KafkaProducer>(provider =>
        {
            var kafkaConfig = provider.GetRequiredService<IOptions<KafkaConfiguration>>().Value;
            return new KafkaProducer(kafkaConfig.BootstrapServers);
        });
        
        return services;
    }
}