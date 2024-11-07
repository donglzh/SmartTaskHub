using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.Service.DependencyInjection;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddServiceService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ITaskTimeoutRuleService, TaskTimeoutRuleService>();
        services.AddScoped<IEquipMaintOrderService, EquipMaintOrderService>();
        services.AddScoped<ITaskRegistrationService, TaskRegistrationService>();
        services.AddScoped<IKafkaService, KafkaService>();
        return services;
    }
}