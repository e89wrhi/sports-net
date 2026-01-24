using Sport.Common.Core;

namespace BuildingBlocks.Contracts.EventBus.Messages;

public record UserCreated(Guid Id, string Name, string PassportNumber) : IIntegrationEvent;