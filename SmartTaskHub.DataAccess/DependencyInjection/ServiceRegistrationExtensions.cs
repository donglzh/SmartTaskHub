using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartTaskHub.Core.Interface;
using SmartTaskHub.DataAccess.Repositories;

namespace SmartTaskHub.DataAccess.DependencyInjection;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddDataAccessService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ITaskTimeoutRuleRepository, TaskTimeoutRuleRepository>();
        services.AddScoped<IEquipMaintOrderRepository, EquipMaintOrderRepository>();
        
        // 注册 DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 28)),
                mysqlOptions => mysqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)));
        
        return services;
    }
}