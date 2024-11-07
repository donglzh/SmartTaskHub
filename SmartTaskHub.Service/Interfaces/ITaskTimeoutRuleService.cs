using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;

namespace SmartTaskHub.Service.Interfaces;

public interface ITaskTimeoutRuleService
{
    /// <summary>
    /// 添加任务超时规则
    /// </summary>
    /// <param name="request">request</param>
    Task<long> AddTaskTimeoutRuleAsync(CreateTaskTimeoutRuleRequest request);
    
    /// <summary>
    /// 删除任务超时规则
    /// </summary>
    /// <param name="id">id</param>
    Task DeleteTaskTimeoutRuleAsync(long id);
    
    /// <summary>
    /// 查询任务超时规则
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    Task<TaskTimeoutRuleDto> GetTaskTimeoutRuleAsync(long id);
    
    /// <summary>
    /// 查询任务超时规则
    /// </summary>
    /// <param name="taskType">任务类型</param>
    /// <returns></returns>
    Task<TaskTimeoutRuleDto> GetTaskTimeoutRuleByTaskTypeAsync(string taskType);
    
    /// <summary>
    /// 查询任务超时规则
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TaskTimeoutRuleDto>> GetAllTaskTimeoutRulesAsync();
}