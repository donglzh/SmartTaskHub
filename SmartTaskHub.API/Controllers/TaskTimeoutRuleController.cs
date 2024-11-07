using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;
using SmartTaskHub.Service.DTOs.Responses;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TaskTimeoutRuleController : Controller
{
    private readonly ITaskTimeoutRuleService _taskTimeoutRuleService;

    public TaskTimeoutRuleController(ITaskTimeoutRuleService taskTimeoutRuleService)
    {
        _taskTimeoutRuleService = taskTimeoutRuleService;
    }

    /// <summary>
    /// 添加任务超时规则
    /// </summary>
    /// <param name="request">request</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<BaseResponse<TaskTimeoutRuleDto>> Add([FromBody] CreateTaskTimeoutRuleRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.TaskType))
            {
                return new BaseResponse<TaskTimeoutRuleDto>() { Code = 400, Message = "任务类型不可以为空或空值" };
            }

            if (request.TimeoutDurationInMinutes == 0)
            {
                return new BaseResponse<TaskTimeoutRuleDto>() { Code = 400, Message = "超时持续时间不可以为零" };
            }
            
            var id = await _taskTimeoutRuleService.AddTaskTimeoutRuleAsync(request);

            var taskTimeoutRule = await _taskTimeoutRuleService.GetTaskTimeoutRuleAsync(id);

            return new BaseResponse<TaskTimeoutRuleDto>()
            {
                Code = 200,
                Message = "添加成功",
                Data = taskTimeoutRule
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<TaskTimeoutRuleDto>() { Code = 400, Message = ex.Message };
        }
    }
    
    /// <summary>
    /// 删除任务超时规则
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<TaskTimeoutRuleDto>> Delete(long id)
    {
        try
        {
            await _taskTimeoutRuleService.DeleteTaskTimeoutRuleAsync(id);
            return new BaseResponse<TaskTimeoutRuleDto>() { Code = 200, Message = "删除成功" };
        }
        catch (Exception ex)
        {
            return new BaseResponse<TaskTimeoutRuleDto>() { Code = 400, Message = ex.Message };
        }
    }
    

    /// <summary>
    /// 查询任务超时规则
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<TaskTimeoutRuleDto>> GetById(long id)
    {
        var taskTimeoutRule = await _taskTimeoutRuleService.GetTaskTimeoutRuleAsync(id);
        if (taskTimeoutRule == null)
        {
            return new BaseResponse<TaskTimeoutRuleDto>() { Code = 400, Message = $"未找到 {id} 任务超时规则" };
        }
        return new BaseResponse<TaskTimeoutRuleDto>() { Code = 200, Message = "成功", Data = taskTimeoutRule };
    }

    /// <summary>
    /// 查询所有的任务超时规则
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<IEnumerable<TaskTimeoutRuleDto>>> GetAll()
    {
        var orders = await _taskTimeoutRuleService.GetAllTaskTimeoutRulesAsync();
        return new BaseResponse<IEnumerable<TaskTimeoutRuleDto>>() { Code = 200, Message = "成功", Data = orders };
    }
}