namespace Vote.Votes.Models;

public class VoteReadModel
{
    public required Guid Id { get; init; }
    public required Guid VoterId { get; init; }
    public required Guid MatchId { get; init; }
    public required string Type { get; init; }
    public required DateTime VotedAt { get; init; }

}
