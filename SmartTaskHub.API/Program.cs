using Microsoft.EntityFrameworkCore;
using SmartTaskHub.Core.Snowflake;
using SmartTaskHub.DataAccess;
using SmartTaskHub.Core.DependencyInjection;
using SmartTaskHub.DataAccess.DependencyInjection;
using SmartTaskHub.Infrastructure.DependencyInjection;
using SmartTaskHub.Service.DependencyInjection;

namespace SmartTaskHub.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // 注册 Core 服务
        builder.Services.AddCoreService(builder.Configuration);
        // 注册 DataAccess 服务
        builder.Services.AddDataAccessService(builder.Configuration);
        // 注册 Infrastructure 服务
        builder.Services.AddInfrastructureService(builder.Configuration);
        // 注册 Service 服务
        builder.Services.AddServiceService(builder.Configuration);
        
        //builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();
        
        app.Run();
    }
}