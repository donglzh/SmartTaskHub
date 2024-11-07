using StackExchange.Redis;

namespace SmartTaskHub.Infrastructure.Redis;

/// <summary>
/// Redis 连接管理
/// </summary>
public class RedisConnectionManager
{
    // ConnectionMultiplexer 实例在应用程序中是单例的，因为它是线程安全的，并且创建连接的开销较大。
    private readonly ConnectionMultiplexer _connection;

    public RedisConnectionManager(string connectionString)
    {
        // 创建一个 ConnectionMultiplexer 实例
        _connection = ConnectionMultiplexer.Connect(connectionString);
    }

    /// <summary>
    /// 获取 Redis 数据库实例
    /// </summary>
    /// <returns></returns>
    public IDatabase GetDatabase()
    {
        return _connection.GetDatabase();
    }

    /// <summary>
    /// 获取 Redis 服务器信息
    /// </summary>
    /// <param name="host">主机名</param>
    /// <param name="port">端口</param>
    /// <returns></returns>
    public IServer GetServer(string host, int port)
    {
        return _connection.GetServer(host, port);
    }
    
    /// <summary>
    /// 获取 Redis 连接的状态
    /// </summary>
    /// <returns></returns>
    public string GetStatus()
    {
        return _connection.GetStatus();
    }

    /// <summary>
    /// 关闭 Redis 连接
    /// </summary>
    public void Close()
    {
        _connection.Close();
    }
}