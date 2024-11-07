using System.ComponentModel.DataAnnotations;

namespace SmartTaskHub.Service.DTOs.Requests;

public class CreateTaskTimeoutRuleRequest
{
    /// <summary>
    /// 任务类型
    /// </summary>
    [Required]
    public string TaskType { get; set; }
    
    /// <summary>
    /// 超时持续时间(分钟)
    /// </summary>
    [Required]
    public long TimeoutDurationInMinutes { get; set; }
}