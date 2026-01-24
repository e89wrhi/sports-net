using Sport.Common.Core;

namespace Sport.Common.Contracts.EventBus.Messages;

public record EventCreated(Guid Id) : IIntegrationEvent;
public record EventDeleted(Guid Id) : IIntegrationEvent;