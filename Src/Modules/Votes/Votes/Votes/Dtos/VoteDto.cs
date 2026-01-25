namespace Vote.Votes.Dtos;

public record VoteDto(Guid Id, Guid VoterId, Guid MatchId, string Type, DateTime VotedAt);
