using Sport.Common.Core;

namespace Sport.Common.Contracts.EventBus.Messages;

public record EventCreatedIntegrationEvent(
    Guid EventId,
    Guid MatchId,
    string Title,
    string Time,
    int EventType) : IIntegrationEvent;
