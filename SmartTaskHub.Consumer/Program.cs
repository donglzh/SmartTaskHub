using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartTaskHub.Core.DependencyInjection;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.DataAccess.DependencyInjection;
using SmartTaskHub.Infrastructure.DependencyInjection;
using SmartTaskHub.Infrastructure.Redis;
using SmartTaskHub.Service.DependencyInjection;
using SmartTaskHub.Service.DTOs;

namespace SmartTaskHub.Consumer;

class Program
{
    static async Task Main(string[] args)
    {
        // 创建主机
        var host = CreateHostBuilder(args).Build();
        
        // 获取 KafkaConsumer 实例
        var kafkaConsumer = host.Services.GetRequiredService<KafkaConsumer>();
    
        using var cts = new CancellationTokenSource();
        
        // 设置 Ctrl+C 处理程序
        Console.CancelKeyPress += (s, e) =>
        {
            // 防止控制台关闭
            e.Cancel = true;
            // 请求取消
            cts.Cancel();
        };
        
        // 启动 kafka 消费者服务
        await kafkaConsumer.StartAsync(cts.Token);
        
        // 启动主机，开始处理请求
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // 注册 AutoMapper
                services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                
                // 注册 Core 层服务
                services.AddCoreService(hostContext.Configuration);
                
                // 注册 DataAccess 层服务
                services.AddDataAccessService(hostContext.Configuration);
                
                // 注册 Infrastructure 层服务
                services.AddInfrastructureService(hostContext.Configuration);
                
                // 注册 Service 层服务
                services.AddServiceService(hostContext.Configuration);

                // 注册 KafkaConsumer, 应用的核心服务
                services.AddSingleton<KafkaConsumer>();
            });
}

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<TaskDetailsDto, TaskDetails>();
        CreateMap<TaskTimeoutRuleDto, TaskTimeoutRule>();
    }
}