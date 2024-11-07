namespace SmartTaskHub.Service.DTOs.Requests;

/// <summary>
/// 创建设备维修工单 Request
/// </summary>
public class CreateEquipMaintOrderRequest
{
    /// <summary>
    /// 编号
    /// </summary>
    public string  Code { get; set; }
    
    /// <summary>
    /// 计划完成时间
    /// </summary>
    public DateTime PlannedCompletionTime { get; set; }
}