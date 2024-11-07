using AutoMapper;
using Newtonsoft.Json;
using SmartTaskHub.Core.Common;
using SmartTaskHub.Core.Entity;
using SmartTaskHub.Core.Interface;
using SmartTaskHub.Infrastructure.Redis;
using SmartTaskHub.Infrastructure.Redis.Interface;
using SmartTaskHub.Service.Interfaces;
using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.Service;

/// <summary>
/// EquipMaintOrderService
/// </summary>
public class EquipMaintOrderService : IEquipMaintOrderService
{
    private readonly IEquipMaintOrderRepository _equipMaintOrderRepository;
    private readonly IRedisString _redisString;
    private readonly IRedisSortedSet _redisSortedSet;
    private readonly IMapper _mapper;
    
    public EquipMaintOrderService(IEquipMaintOrderRepository equipMaintOrderRepository, 
        IRedisString redisString, 
        IRedisSortedSet redisSortedSet,
        IMapper mapper)
    {
        _equipMaintOrderRepository = equipMaintOrderRepository;
        _redisString = redisString;
        _redisSortedSet = redisSortedSet;
        _mapper = mapper;
    }
    
    /// <summary>
    /// 添加设备维修工单
    /// </summary>
    /// <param name="request">request</param>
    public async Task<long> AddOrderAsync(CreateEquipMaintOrderRequest request)
    {
        try
        {
            var order = _mapper.Map<EquipMaintOrder>(request);
            order.IsCompleted = false;
            order.PlannedCompletionTime = request.PlannedCompletionTime.ToTimestamp();
            await _equipMaintOrderRepository.AddAsync(order);

            // 将设备维修工单任务详情存储到 Redis 中
            var taskDetails = new TaskDetails()
            {
                Code = order.Code,
                TaskType = TaskType.Equipment.ToString(),
                Level = TaskLevel.Level1,
                ScheduledTime = order.PlannedCompletionTime
            };
            var value = JsonConvert.SerializeObject(taskDetails);
            await _redisString.SetAsync(order.Id.ToString(),value);
       
            // 将设备维修工单任务添加到 Redis 队列中
            await _redisSortedSet.AddAsync(order.Id.ToString(), order.PlannedCompletionTime);

            return order.Id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 删除设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    public async Task<bool> DeleteOrderAsync(long id)
    {
        try
        {
            var order = await _equipMaintOrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception($"未找到 {id} 设备维修工单");
            }
            _equipMaintOrderRepository.Delete(order);
        
            // 将设备维修工单任务详情从 Redis 中删除
            await _redisString.DeleteAsync(order.Id.ToString());
        
            // 将设备维修工单任务从 Redis 队列中移除
            await _redisSortedSet.RemoveAsync(order.Id.ToString());
            
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 完成设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    public async Task<bool> CompleteOrderAsync(long id)
    {
        try
        {
            var order = await _equipMaintOrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception($"未找到 {id} 设备维修工单");
            }
            order.IsCompleted = true;
            _equipMaintOrderRepository.Update(order);
        
            // 将设备维修工单任务详情从 Redis 中删除
            await _redisString.DeleteAsync(order.Id.ToString());
        
            // 将设备维修工单任务从 Redis 队列中移除
            await _redisSortedSet.RemoveAsync(order.Id.ToString());
            
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }

    /// <summary>
    /// 查询设备维修工单
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    public async Task<EquipMaintOrderDto> GetOrderAsync(long id)
    {
        var order = await _equipMaintOrderRepository.GetByIdAsync(id);
        return _mapper.Map<EquipMaintOrderDto>(order);
    }
    
    /// <summary>
    /// 查询设备维修工单
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<EquipMaintOrderDto>> GetAllOrdersAsync()
    {
        var orders = await _equipMaintOrderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<EquipMaintOrderDto>>(orders);
    }

    /// <summary>
    /// 查询未完成的设备维修工单
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<EquipMaintOrderDto>> GetPendingOrdersAsync()
    {
        var orders = await _equipMaintOrderRepository.GetPendingOrdersAsync();
        return _mapper.Map<IEnumerable<EquipMaintOrderDto>>(orders);
    }
}