namespace Event.Events.Dtos;

public record EventDto(Guid Id, Guid MatchId, string Title, DateTime Time, Enums.EventType Type);