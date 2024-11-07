namespace SmartTaskHub.Core.Entity;

/// <summary>
/// 任务超时规则
/// </summary>
public class TaskTimeoutRule : BaseEntity
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public string TaskType { get; set; }
    
    /// <summary>
    /// 超时持续时间
    /// </summary>
    public TimeSpan TimeoutDuration { get; set; }
}