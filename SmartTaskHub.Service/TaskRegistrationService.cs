using AutoMapper;
using Newtonsoft.Json;
using SmartTaskHub.Core.Common;
using SmartTaskHub.Infrastructure.Redis;
using SmartTaskHub.Infrastructure.Redis.Interface;
using SmartTaskHub.Service.DTOs;
using SmartTaskHub.Service.DTOs.Requests;
using SmartTaskHub.Service.Interfaces;

namespace SmartTaskHub.Service;

public class TaskRegistrationService : ITaskRegistrationService
{
    private readonly IRedisString _redisString;
    private readonly IRedisSortedSet _redisSortedSet;
    private readonly IMapper _mapper;

    public TaskRegistrationService(IRedisString redisString, IRedisSortedSet redisSortedSet,
        IMapper mapper)
    {
        _redisString = redisString;
        _redisSortedSet = redisSortedSet;
        _mapper = mapper;
    }
    
    /// <summary>
    /// 注册任务
    /// </summary>
    /// <param name="request">request</param>
    public async Task<bool> RegisterTaskAsync(RegisterTaskRequest request)
    {
        try
        {
            if (await _redisSortedSet.SortedSetRankAsync(request.Key))
            {
                throw new Exception($"Redis 队列中 {request.Key} 任务已存在");
            }

            var taskDetails = new TaskDetails()
            {
                Code = request.Code,
                TaskType = request.TaskType,
                Level = request.Level,
                ScheduledTime = request.ScheduledTime,
            };
            var value = JsonConvert.SerializeObject(taskDetails);
        
            // 将 value 存储到 Redis 中
            await _redisString.SetAsync(request.Key, value);
       
            // 将 key(task) 添加到 Redis 队列中
            await _redisSortedSet.AddAsync(request.Key, request.ScheduledTime);
        
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 注销任务
    /// </summary>
    /// <param name="key">唯一标识符</param>
    public async Task<bool> UnregisterTaskAsync(string key)
    {
        try
        {
            if (!await _redisSortedSet.SortedSetRankAsync(key))
            {
                throw new Exception($"Redis 队列中 {key} 任务不存在，无需注销");
            }
        
            // 将设备维修工单任务详情从 Redis 中删除
            await _redisString.DeleteAsync(key);
        
            // 将设备维修工单任务从 Redis 队列中移除
            await _redisSortedSet.RemoveAsync(key);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 获取任务详情
    /// </summary>
    /// <param name="key">唯一标识符</param>
    /// <returns></returns>
    public async Task<TaskDetailsDto> GetTaskDetailsAsync(string key)
    {
        if (!await _redisString.ExistsAsync(key))
        {
            throw new Exception($"Redis 中 {key} 任务详情不存在");
        }
        var taskDetails = await _redisString.GetAsync(key);
        return JsonConvert.DeserializeObject<TaskDetailsDto>(taskDetails) ?? new TaskDetailsDto();
    }

    /// <summary>
    /// 修改任务详情
    /// </summary>
    /// <param name="key">唯一标识符</param>
    /// <param name="value">值</param>
    public async Task<bool> UpdateTaskDetailsAsync(string key, TaskDetailsDto value)
    {
        try
        {
            if (!await _redisString.ExistsAsync(key))
            {
                throw new Exception($"Redis 中 {key} 任务详情不存在");
            }
            var taskDetails = _mapper.Map<TaskDetails>(value);
            var v = JsonConvert.SerializeObject(taskDetails);
            await _redisString.SetAsync(key, v);
            
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }

    /// <summary>
    /// 删除任务详情
    /// </summary>
    /// <param name="key">唯一标识符</param>
    /// <returns></returns>
    public async Task<bool> DeleteTaskDetailsAsync(string key)
    {
        try
        {
            if (!await _redisString.ExistsAsync(key))
            {
                throw new Exception($"Redis 中 {key} 任务详情不存在");
            }
            await _redisString.DeleteAsync(key);
            
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    /// <summary>
    /// 将任务从队列中移除
    /// </summary>
    /// <param name="key">唯一标识符</param>
    public async Task<bool> RemoveFromQueueAsync(string key)
    {
        try
        {
            if (!await _redisSortedSet.SortedSetRankAsync(key))
            {
                throw new Exception($"Redis 队列中 {key} 任务不存在，无需移除");
            }
            await _redisSortedSet.RemoveAsync(key);
            
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 获取所有过期任务
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string>> GetOverdueTasksAsync()
    {
        var results = await _redisSortedSet.GetRangeByScoreAsync(0, TimestampHelper.GetCurrentTimestamp());
        return results.Select(x=>x.ToString());
    }
}