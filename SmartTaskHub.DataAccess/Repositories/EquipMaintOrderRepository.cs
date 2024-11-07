using Microsoft.EntityFrameworkCore;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.Core.Interface;

namespace SmartTaskHub.DataAccess.Repositories;

/// <summary>
/// EquipMaintOrderRepository
/// </summary>
public class EquipMaintOrderRepository : Repository<EquipMaintOrder>, IEquipMaintOrderRepository
{
    public EquipMaintOrderRepository(AppDbContext context) : base(context) {}

    /// <summary>
    /// 查询未完成的设备维修工单
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<EquipMaintOrder>> GetPendingOrdersAsync()
    {
        return await _dbSet.Where(x => !x.IsCompleted).ToListAsync();
    }
}