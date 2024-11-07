namespace SmartTaskHub.Service.DTOs;

/// <summary>
/// EquipMaintOrderDto
/// </summary>
public class EquipMaintOrderDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    
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
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public long CreatedAt { get; set; }
    
    /// <summary>
    /// 更新时间
    /// </summary>
    public long UpdatedAt { get; set; }
}