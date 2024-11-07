using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;

namespace SmartTaskHub.Service.Interfaces;

public interface ITaskRegistrationService
{
    /// <summary>
    /// 注册任务
    /// </summary>
    /// <param name="request">request</param>
    Task<bool> RegisterTaskAsync(RegisterTaskRequest request);
    
    /// <summary>
    /// 注销任务
    /// </summary>
    /// <param name="key">唯一标识符</param>
    Task<bool> UnregisterTaskAsync(string key);
    
    /// <summary>
    /// 获取任务详情
    /// </summary>
    /// <param name="key">唯一标识符</param>
    /// <returns></returns>
    Task<TaskDetailsDto> GetTaskDetailsAsync(string key);
    
    /// <summary>
    /// 修改任务详情
    /// </summary>
    /// <param name="key">唯一标识符</param>
    /// <param name="value">值</param>
    Task<bool> UpdateTaskDetailsAsync(string key, TaskDetailsDto value);
    
    /// <summary>
    /// 删除任务详情
    /// </summary>
    /// <param name="key">唯一标识符</param>
    /// <returns></returns>
    Task<bool> DeleteTaskDetailsAsync(string key);
    
    /// <summary>
    /// 将任务从队列中移除
    /// </summary>
    /// <param name="key">唯一标识符</param>
    Task<bool> RemoveFromQueueAsync(string key);

    /// <summary>
    /// 获取所有过期任务
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string>> GetOverdueTasksAsync();
}