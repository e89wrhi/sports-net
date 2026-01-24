using Sport.Common.Core;

namespace Sport.Common.Contracts.EventBus.Messages;

public record VoteCreatedIntegrationEvent(
    Guid VoteId,
    Guid MatchId,
    int VoteType) : IIntegrationEvent; 
