namespace SmartTaskHub.Core.Entity;

/// <summary>
/// 设备维修工单
/// </summary>
public class EquipMaintOrder : BaseEntity
{
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsCompleted { get; set; }
    
    /// <summary>
    /// 计划完成时间
    /// </summary>
    public long PlannedCompletionTime { get; set; }
}