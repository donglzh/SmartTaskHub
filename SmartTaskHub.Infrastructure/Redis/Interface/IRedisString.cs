namespace SmartTaskHub.Infrastructure.Redis.Interface;

/// <summary>
/// Redis 字符串数据类型接口
/// </summary>
public interface IRedisString
{
    /// <summary>
    /// 设置指定键的值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expiry">过期时间</param>
    /// <returns></returns>
    Task SetAsync(string key, string value, TimeSpan? expiry = null);
    
    /// <summary>
    /// 获取指定键的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    Task<string> GetAsync(string key);
    
    /// <summary>
    /// 删除指定键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(string key);
    
    /// <summary>
    /// 检查键是否存在
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(string key);
}

