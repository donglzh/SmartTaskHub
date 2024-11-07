using SmartTaskHub.Core.Entity;

namespace SmartTaskHub.Core.Interface;

/// <summary>
/// ITaskTimeoutRuleRepository
/// </summary>
public interface ITaskTimeoutRuleRepository : IRepository<TaskTimeoutRule>
{
    /// <summary>
    /// 根据 taskType 查询任务超时规则
    /// </summary>
    /// <param name="taskType">任务类型</param>
    /// <returns></returns>
    Task<TaskTimeoutRule> GetByTaskTypeAsync(string taskType);
}