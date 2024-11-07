using StackExchange.Redis;

namespace SmartTaskHub.Infrastructure.Redis.Interface;

/// <summary>
/// Redis 有序集合(sorted set)
/// </summary>
public interface IRedisSortedSet
{
    /// <summary>
    /// 将 number 和 score 添加到 key 的有序集合中
    /// </summary>
    /// <param name="number">number</param>
    /// <param name="score">score</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    Task<bool> AddAsync(string number, double score,string? key = null);
    
    /// <summary>
    /// 将 number 从 key 的有序集合中移除
    /// </summary>
    /// <param name="number">number</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    Task<bool> RemoveAsync(string number, string? key = null);
    
    /// <summary>
    /// 按 score 区间获取 key 有序集合的 number
    /// </summary>
    /// <param name="min">min</param>
    /// <param name="max">max</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    Task<RedisValue[]> GetRangeByScoreAsync(double min, double max, string? key = null);
    
    /// <summary>
    /// 检查 key 的有序集合中 number 是否存在
    /// </summary>
    /// <param name="member">number</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    Task<bool> SortedSetRankAsync(string member,string? key = null);
}