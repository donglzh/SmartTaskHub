using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;
using SmartTaskHub.Service.DTOs.Responses;

namespace SmartTaskHub.Service.Interfaces;

public interface IEquipMaintOrderService
{
    /// <summary>
    /// 添加设备维修工单
    /// </summary>
    /// <param name="request">request</param>
    Task<long> AddOrderAsync(CreateEquipMaintOrderRequest request);
    
    /// <summary>
    /// 删除设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    Task<bool> DeleteOrderAsync(long id);
    
    /// <summary>
    /// 完成设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    Task<bool> CompleteOrderAsync(long id);
    
    /// <summary>
    /// 查询设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    Task<EquipMaintOrderDto> GetOrderAsync(long id);
    
    /// <summary>
    /// 查询设备维修工单
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<EquipMaintOrderDto>> GetAllOrdersAsync();
    
    /// <summary>
    /// 查询未完成的设备维修工单
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<EquipMaintOrderDto>> GetPendingOrdersAsync();
}