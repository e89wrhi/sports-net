namespace Events.Models;

public class EventReadModel
{
    public required Guid Id { get; init; }
    public required Guid MatchId { get; init; }
    public required string Title { get; init; }
    public required DateTime Time { get; init; }
    public required string Type { get; init; }

}
