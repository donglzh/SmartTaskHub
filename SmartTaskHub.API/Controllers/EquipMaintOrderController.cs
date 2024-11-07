using Microsoft.AspNetCore.Mvc;
using SmartTaskHub.Core.Common;
using SmartTaskHub.Service.Interfaces;
using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;
using SmartTaskHub.Service.DTOs.Responses;

namespace SmartTaskHub.API.Controllers;


[Route("api/[controller]/[action]")]
[ApiController]
public class EquipMaintOrderController : Controller
{
    private readonly IEquipMaintOrderService _equipMaintOrderService;
    
    public EquipMaintOrderController(IEquipMaintOrderService equipMaintOrderService)
    {
        _equipMaintOrderService = equipMaintOrderService;
    }
    
    /// <summary>
    /// 添加设备维修工单
    /// </summary>
    /// <param name="request">request</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<BaseResponse<EquipMaintOrderDto>> AddOrder([FromBody] CreateEquipMaintOrderRequest request)
    {
        try
        {
            if (request == null)
            {
                return new BaseResponse<EquipMaintOrderDto>() { Code = 400, Message = "设备维修工单表单不可以为空" };
            }
            var id = await _equipMaintOrderService.AddOrderAsync(request);

            var order = await _equipMaintOrderService.GetOrderAsync(id);
            
            return new BaseResponse<EquipMaintOrderDto>()
            {
                Code = 200,
                Message = "添加成功",
                Data = order
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<EquipMaintOrderDto>() { Code = 400, Message = ex.Message };
        }
    }
    
    /// <summary>
    /// 删除设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<EquipMaintOrderDto>> DeleteOrder(long id)
    {
        try
        {
            await _equipMaintOrderService.DeleteOrderAsync(id);
            return new BaseResponse<EquipMaintOrderDto>() { Code = 200, Message = "删除成功" };
        }
        catch (Exception ex)
        {
            return new BaseResponse<EquipMaintOrderDto>() { Code = 400, Message = ex.Message };
        }
    }
    
    /// <summary>
    /// 完成设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<EquipMaintOrderDto>> CompleteOrder(long id)
    {
        try
        {
            await _equipMaintOrderService.CompleteOrderAsync(id);
            return new BaseResponse<EquipMaintOrderDto>() { Code = 200, Message = "成功" };
        }
        catch (Exception ex)
        {
            return new BaseResponse<EquipMaintOrderDto>() { Code = 400, Message = ex.Message };
        }
    }
    
    /// <summary>
    /// 根据 Id 查询设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<EquipMaintOrderDto>> GetById(long id)
    {
        var order = await _equipMaintOrderService.GetOrderAsync(id);
        if (order == null)
        {
            return new BaseResponse<EquipMaintOrderDto>() { Code = 400, Message = $"未找到 {id} 设备维修工单" };
        }
        return new BaseResponse<EquipMaintOrderDto>() { Code = 200, Message = "成功", Data = order };
    }
    
    /// <summary>
    /// 查询所有的设备维修工单
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<IEnumerable<EquipMaintOrderDto>>> GetAllOrders()
    {
        var orders = await _equipMaintOrderService.GetAllOrdersAsync();
        return new BaseResponse<IEnumerable<EquipMaintOrderDto>>() { Code = 200, Message = "成功", Data = orders };
    }
    
    /// <summary>
    /// 查询所有未完成的设备维修工单
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseResponse<IEnumerable<EquipMaintOrderDto>>> GetPendingOrders()
    {
        var orders = await _equipMaintOrderService.GetPendingOrdersAsync();
        return new BaseResponse<IEnumerable<EquipMaintOrderDto>>() { Code = 200, Message = "成功", Data = orders };
    }
}