using System.ComponentModel;

namespace SmartTaskHub.Infrastructure.Redis;

/// <summary>
/// 任务类型
/// </summary>
public enum TaskType
{
    [Description("Equipment")]
    Equipment,
    
    [Description("OutOfStock")]
    OutOfStock
}