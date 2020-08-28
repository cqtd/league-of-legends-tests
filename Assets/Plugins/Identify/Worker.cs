using System;

/// <summary>
/// Snowflake
/// </summary>
public class Worker
{
    public const long Twepoch = 1246104083991L;

    const int workerIdBits = 12;
    const int datacenterIdBits = 2;
    const int sequenceBits = 8;
    const long maxWorkerId = -1L ^ (-1L << workerIdBits);
    const long maxDatacenterId = -1L ^ (-1L << datacenterIdBits);

    const int workerIdShift = sequenceBits;
    const int datacenterIdShift = sequenceBits + workerIdBits;
    const int timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;

    const long sequenceMask = -1L ^ (-1L << sequenceBits);
    
    long _sequence = 0L;
    long _lastTimestamp = -1L;

    public Worker(long datacenterId, long workerId, long sequence = 0L)
    {
        WorkerId = workerId;
        DatacenterId = datacenterId;
        _sequence = sequence;

        if (workerId > maxWorkerId || workerId < 0) {
            throw new ArgumentException($"worker Id can't be greater than {maxWorkerId} or less than 0");
        }

        if (datacenterId > maxDatacenterId || datacenterId < 0) {
            throw new ArgumentException($"datacenter Id can't be greater than {maxDatacenterId} or less than 0");
        }
    }

    public long WorkerId { get; protected set; }

    public long DatacenterId { get; protected set; }

    public long Sequence {
        get { return _sequence; }
        internal set { _sequence = value; }
    }

    readonly object _lock = new Object();

    public virtual long NextId()
    {
        lock (_lock) {
            var timestamp = TimeGen();

            if (timestamp < _lastTimestamp) {
                throw new InvalidSystemClockException(
                    $"Clock moved backwards.  Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
            }

            if (_lastTimestamp == timestamp) {
                _sequence = (_sequence + 1) & sequenceMask;
                if (_sequence == 0) {
                    timestamp = TilNextMillis(_lastTimestamp);
                }
            } else {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;
            var id = ((timestamp - Twepoch) << timestampLeftShift) |
                     (DatacenterId << datacenterIdShift) |
                     (WorkerId << workerIdShift) | _sequence;

            return id;
        }
    }

    protected virtual long TilNextMillis(long lastTimestamp)
    {
        var timestamp = TimeGen();
        while (timestamp <= lastTimestamp) {
            timestamp = TimeGen();
        }
        return timestamp;
    }

    protected virtual long TimeGen()
    {
        return TimeUtility.CurrentTimeMillis();
    }
}
