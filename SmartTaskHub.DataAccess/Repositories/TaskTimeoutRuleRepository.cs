using Microsoft.EntityFrameworkCore;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.Core.Interface;

namespace SmartTaskHub.DataAccess.Repositories;

/// <summary>
/// TaskTimeoutRuleRepository
/// </summary>
public class TaskTimeoutRuleRepository : Repository<TaskTimeoutRule>, ITaskTimeoutRuleRepository
{
    public TaskTimeoutRuleRepository(AppDbContext context) : base(context){}
    
    /// <summary>
    /// 根据 taskType 查询任务超时规则
    /// </summary>
    /// <param name="taskType">任务类型</param>
    /// <returns></returns>
    public async Task<TaskTimeoutRule> GetByTaskTypeAsync(string taskType)
    {
        return await _dbSet.Where(x => x.TaskType == taskType).FirstOrDefaultAsync();
    }
}