namespace SmartTaskHub.Core.Entity;

/// <summary>
/// 基类
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public long CreatedAt { get; set; }
    
    /// <summary>
    /// 更新时间
    /// </summary>
    public long UpdatedAt { get; set; }
}