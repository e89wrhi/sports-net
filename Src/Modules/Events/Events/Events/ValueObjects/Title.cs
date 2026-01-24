using Event.Events.Exeptions;

namespace Event.Events.ValueObjects;

public record Title
{
    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Title Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidTitleException(value);
        }

        return new Title(value);
    }

    public static implicit operator string(Title title)
    {
        return title.Value;
    }
}