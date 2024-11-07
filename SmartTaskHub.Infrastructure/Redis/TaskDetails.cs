namespace SmartTaskHub.Infrastructure.Redis;

/// <summary>
/// 计划任务详情
/// </summary>
public class TaskDetails
{
    /// <summary>
    /// 任务编号
    /// </summary>
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// 任务类型
    /// </summary>
    public string TaskType { get; set; } = string.Empty;
    
    /// <summary>
    /// 级别
    /// </summary>
    public TaskLevel Level { get; set; }
    
    /// <summary>
    /// 预定执行时间
    /// </summary>
    public long ScheduledTime { get; set; }
}