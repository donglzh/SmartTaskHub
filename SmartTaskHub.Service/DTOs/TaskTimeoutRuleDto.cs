namespace SmartTaskHub.Service.DTOs;

/// <summary>
/// TaskTimeoutRuleDto
/// </summary>
public class TaskTimeoutRuleDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public string TaskType { get; set; }
    
    /// <summary>
    /// 超时时长
    /// </summary>
    public TimeSpan TimeoutDuration { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public long CreatedAt { get; set; }
    
    /// <summary>
    /// 更新时间
    /// </summary>
    public long UpdatedAt { get; set; }
}