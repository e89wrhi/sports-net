using Sport.Common.Core;

namespace Sport.Contracts.EventBus.Messages;

public record UserCreated(Guid Id, string Name, string PassportNumber) : IIntegrationEvent;