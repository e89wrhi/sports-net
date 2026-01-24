using Sport.Common.Core;

namespace Sport.Common.Contracts.EventBus.Messages;

public record VoteCreated(Guid Id) : IIntegrationEvent;
public record VoteDeleted(Guid Id) : IIntegrationEvent;