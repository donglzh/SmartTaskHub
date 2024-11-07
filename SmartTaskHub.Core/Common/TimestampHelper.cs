namespace SmartTaskHub.Core.Common;

public static class TimestampHelper
{
    /// <summary>
    /// 获取当前时间戳（秒）
    /// </summary>
    /// <returns>返回当前时间戳</returns>
    public static long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// DateTime 转换为时间戳（秒）
    /// </summary>
    /// <param name="dateTime">日期时间</param>
    /// <returns>返回转换后的时间戳</returns>
    public static long ToTimestamp(this DateTime dateTime)
    {
        // 确保 dateTime 是 UTC 时间
        if (dateTime.Kind == DateTimeKind.Unspecified)
        {
            // 如果未指定，可以假设是 UTC
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    
        // 如果是本地时间，则转换为 UTC
        if (dateTime.Kind == DateTimeKind.Local)
        {
            dateTime = dateTime.ToUniversalTime();
        }
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }
    
    /// <summary>
    /// 时间戳（秒）转换为 DateTime
    /// </summary>
    /// <param name="timestamp">时间戳（秒）</param>
    /// <returns>返回转换后的日期时间</returns>
    public static DateTime FromTimestamp(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
    }
}