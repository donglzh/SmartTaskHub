using Microsoft.AspNetCore.Mvc;
using SmartTaskHub.Infrastructure.Redis;
using SmartTaskHub.Service.DTOs.Requests;
using SmartTaskHub.Service.DTOs.Responses;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TaskRegistrationController : Controller
{
    private readonly ITaskRegistrationService _taskRegistrationService;

    public TaskRegistrationController(ITaskRegistrationService taskRegistrationService)
    {
        _taskRegistrationService = taskRegistrationService;
    }


    /// <summary>
    /// 注册任务
    /// </summary>
    /// <param name="request">request</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<BaseResponse<bool>> Register([FromQuery] RegisterRequest request)
    {
        try
        {
            if (request == null)
            {
                return new BaseResponse<bool>() { Code = 400, Message = "任务表单不可以为空" };
            }
    
            var value = new RegisterTaskRequest()
            {
                Key = request.Key,
                Code = request.Value,
                TaskType = request.TaskType,
                Level = TaskLevel.Level1,
                ScheduledTime = request.ScheduledTime
            };
            
            await _taskRegistrationService.RegisterTaskAsync(value);
            return new BaseResponse<bool>() { Code = 200, Message = "成功" };
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>() { Code = 400, Message = ex.Message };
        }
    }
    
    /// <summary>
    /// 取消注册任务
    /// </summary>
    /// <param name="key">唯一标识符</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<bool>> Unregister(string key)
    {
        try
        {
            if (string.IsNullOrEmpty(key))
            {
                return new BaseResponse<bool>() { Code = 400, Message = "唯一标识符不可以为空或空值" };
            }
            await _taskRegistrationService.UnregisterTaskAsync(key);
            return new BaseResponse<bool>() { Code = 200, Message = "成功" };
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>() { Code = 400, Message = ex.Message };
        }
    }

    [HttpGet]
    public async Task Check()
    {
        var overdueTasks = await _taskRegistrationService.GetOverdueTasksAsync();
        foreach (var VARIABLE in overdueTasks)
        {
            
        }
    }
}




/// <summary>
/// 注册任务请求
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public string TaskType { get; set; }
    
    /// <summary>
    /// 唯一标识符
    /// </summary>
    public string Key { get; set; }
    
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }
    
    /// <summary>
    /// 预定执行时间
    /// </summary>
    public long ScheduledTime { get; set; }
}