using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartTaskHub.Core.DependencyInjection;
using SmartTaskHub.DataAccess.DependencyInjection;
using SmartTaskHub.Infrastructure.DependencyInjection;
using SmartTaskHub.Service.DependencyInjection;

namespace SmartTaskHub.Poller;

class Program
{
    static async Task Main(string[] args)
    {
        // 创建主机
        var host = CreateHostBuilder(args).Build();
        
        // 获取 TaskPoller 实例
        var taskPoller = host.Services.GetRequiredService<TaskPoller>();

        using var cts = new CancellationTokenSource();
        
        // 设置 Ctrl+C 处理程序
        Console.CancelKeyPress += (s, e) =>
        {
            // 防止控制台关闭
            e.Cancel = true;
            // 请求取消
            cts.Cancel();
        };

        // 启动任务轮询
        await taskPoller.StartAsync(cts.Token);
        
        // 启动主机，开始处理请求
        await host.RunAsync();
    }
    
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // 注册 AutoMapper
                services.AddAutoMapper(typeof(Program));
                
                // 注册 Core 层服务
                // services.AddCoreService(hostContext.Configuration);
                
                // 注册 DataAccess 层服务
                // services.AddDataAccessService(hostContext.Configuration);
                
                // 注册 Infrastructure 层服务
                services.AddInfrastructureService(hostContext.Configuration);
                
                // 注册 Service 层服务
                services.AddServiceService(hostContext.Configuration);

                // 注册 TaskPoller，应用的核心服务
                services.AddSingleton<TaskPoller>();
            });
}