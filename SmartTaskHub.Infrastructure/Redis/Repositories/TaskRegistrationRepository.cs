using SmartTaskHub.Infrastructure.Configuration;
using SmartTaskHub.Infrastructure.Redis.Interface;
using StackExchange.Redis;

namespace SmartTaskHub.Infrastructure.Redis.Repositories;

public class TaskRegistrationRepository :  IRedisString, IRedisSortedSet
{
    private readonly IDatabase _database;
    private readonly RedisConfiguration _config;
    
    public TaskRegistrationRepository(IDatabase database, RedisConfiguration config)
    {
        _database = database;
        _config = config;
    }
    
    /// <summary>
    /// 设置指定键的值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expiry">过期时间</param>
    /// <returns></returns>
    public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        key = GetTaskKey(key);
        await _database.StringSetAsync(key, value, expiry);
    }

    /// <summary>
    /// 获取指定键的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public async Task<string> GetAsync(string key)
    {
        key = GetTaskKey(key);
        return await _database.StringGetAsync(key);
    }

    /// <summary>
    /// 删除指定键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(string key)
    {
        key = GetTaskKey(key);
        return await _database.KeyDeleteAsync(key);
    }

    /// <summary>
    /// 检查键是否存在
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public async Task<bool> ExistsAsync(string key)
    {
        key = GetTaskKey(key);
        return await _database.KeyExistsAsync(key);
    }
    
    /// <summary>
    /// 将 number 和 score 添加到 key 的有序集合中
    /// </summary>
    /// <param name="number">number</param>
    /// <param name="score">score</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    public async Task<bool> AddAsync(string number, double score, string? key = null)
    {
        key = _config.ScheduledTaskKey;
        return await _database.SortedSetAddAsync(key, number, score);
    }

    /// <summary>
    /// 将 number 从 key 的有序集合中移除
    /// </summary>
    /// <param name="number">number</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    public async Task<bool> RemoveAsync(string number, string? key = null)
    {
        key = _config.ScheduledTaskKey;
        return await _database.SortedSetRemoveAsync(key, number);
    }

    /// <summary>
    /// 按 score 区间获取 key 有序集合的 number
    /// </summary>
    /// <param name="min">min</param>
    /// <param name="max">max</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    public async Task<RedisValue[]> GetRangeByScoreAsync(double min, double max, string? key = null)
    {
        key = _config.ScheduledTaskKey;
        return await _database.SortedSetRangeByScoreAsync(key,min, max);
    }

    /// <summary>
    /// 检查 key 的有序集合中 number 是否存在
    /// </summary>
    /// <param name="member">number</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    public async Task<bool> SortedSetRankAsync(string member, string? key = null)
    {
        key = _config.ScheduledTaskKey;
        var rank = await _database.SortedSetRankAsync(key, member);
        return rank.HasValue;
    }
    
    /// <summary>
    /// 获取 Task 的 Key
    /// </summary>
    /// <param name="key">key</param>
    /// <returns></returns>
    private string GetTaskKey(string key)
    {
        return $"{_config.TaskKeyPrefix}{key}";
    }
}