using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartTaskHub.Core.Snowflake;

namespace SmartTaskHub.Core.DependencyInjection;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddCoreService(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 注册 Snowflake 配置
        services.Configure<SnowflakeOptions>(configuration.GetSection("Snowflake"));
        services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();
        
        return services;
    }
}