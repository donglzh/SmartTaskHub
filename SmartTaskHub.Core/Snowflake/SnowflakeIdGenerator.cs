using Microsoft.Extensions.Options;

namespace SmartTaskHub.Core.Snowflake;

public class SnowflakeIdGenerator : IIdGenerator
{
    private readonly SnowflakeOptions _options;
    private readonly long _workerId;
    private readonly long _datacenterId;
    private long _lastTimestamp = -1L;
    private const long Twepoch = 1577836800000L; // 2020-01-01 08:00:00
    private const long SequenceBits = 12L;
    private const long WorkerIdBits = 5L;
    private const long DatacenterIdBits = 5L;
    private const long MaxWorkerId = -1L ^ (-1L << (int)WorkerIdBits);
    private const long MaxDatacenterId = -1L ^ (-1L << (int)DatacenterIdBits);
    private const long SequenceMask = -1L ^ (-1L << (int)SequenceBits);
    private const long WorkerIdShift = SequenceBits;
    private const long DatacenterIdShift = SequenceBits + WorkerIdBits;
    private const long TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
    private long _sequence = 0L;
    private readonly object _lock = new object();

    public SnowflakeIdGenerator(IOptions<SnowflakeOptions> options)
    {
        _options = options.Value;
        if (_options.WorkerId > MaxWorkerId || _options.WorkerId < 0)
        {
            throw new ArgumentException($"Worker Id can't be greater than {MaxWorkerId} or less than 0");
        }
        if (_options.DatacenterId > MaxDatacenterId || _options.DatacenterId < 0)
        {
            throw new ArgumentException($"Datacenter Id can't be greater than {MaxDatacenterId} or less than 0");
        }
        _workerId = _options.WorkerId;
        _datacenterId = _options.DatacenterId;
    }

    public long NextId()
    {
        lock (_lock)
        {
            var timestamp = GetCurrentTimestamp();

            if (timestamp < _lastTimestamp)
            {
                // 时间回拨处理
                throw new InvalidOperationException($"Clock moved backwards. Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
            }

            if (_lastTimestamp == timestamp)
            {
                _sequence = (_sequence + 1) & SequenceMask;
                if (_sequence == 0)
                {
                    // 序列号已用尽，等待下一毫秒
                    timestamp = TilNextMillis(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0L;
            }

            _lastTimestamp = timestamp;

            return ((timestamp - Twepoch) << (int)TimestampLeftShift) |
                   (_datacenterId << (int)DatacenterIdShift) |
                   (_workerId << (int)WorkerIdShift) |
                   _sequence;
        }
    }

    private long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    private long TilNextMillis(long lastTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }
}