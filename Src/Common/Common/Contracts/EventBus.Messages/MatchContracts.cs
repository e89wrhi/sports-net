using Sport.Common.Core;

namespace Sport.Common.Contracts.EventBus.Messages;

public record MatchCreated(Guid Id) : IIntegrationEvent;
public record MatchUpdated(Guid Id) : IIntegrationEvent;
public record MatchDeleted(Guid Id) : IIntegrationEvent;
public record MatchScoreUpdated(Guid Id) : IIntegrationEvent;