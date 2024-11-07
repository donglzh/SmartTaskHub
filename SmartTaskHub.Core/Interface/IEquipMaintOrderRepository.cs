using SmartTaskHub.Core.Entity;

namespace SmartTaskHub.Core.Interface;

/// <summary>
/// IEquipMaintOrderRepository
/// </summary>
public interface IEquipMaintOrderRepository : IRepository<EquipMaintOrder>
{
    /// <summary>
    /// 查询未完成的设备维修工单
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<EquipMaintOrder>> GetPendingOrdersAsync();
}