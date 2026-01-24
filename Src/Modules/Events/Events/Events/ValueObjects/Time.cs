using Event.Events.Exeptions;

namespace Event.Events.ValueObjects;

public record Time
{
    public DateTime Value { get; }

    private Time(DateTime value)
    {
        Value = value;
    }

    public static Time Of(DateTime value)
    {
        return new Time(value);
    }

    public static implicit operator DateTime(Time time)
    {
        return time.Value;
    }
}