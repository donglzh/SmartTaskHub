using SmartTaskHub.Infrastructure.Redis;

namespace SmartTaskHub.Service.DTOs.Requests;

public class RegisterTaskRequest
{
    /// <summary>
    /// 唯一标识符
    /// </summary>
    public string Key { get; set; }
    
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// 任务类型
    /// </summary>
    public string TaskType { get; set; }
    
    /// <summary>
    /// 等级
    /// </summary>
    public TaskLevel Level { get; set; }

    /// <summary>
    /// 预定执行时间
    /// </summary>
    public long ScheduledTime { get; set; }
}