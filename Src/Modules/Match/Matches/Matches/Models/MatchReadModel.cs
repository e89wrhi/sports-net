namespace Match.Matches.Models;

public class MatchReadModel
{
    public required Guid Id { get; init; }
    public required string HomeTeam { get; init; }
    public required string AwayTeam { get; init; }
    public required int HomeTeamScore { get; init; }
    public required int AwayTeamScore { get; init; }
    public required string League { get; init; }
    public required string Status { get; init; }
    public required DateTime MatchTime { get; init; }

    public required int EventsCount { get; init; }
    public required int HomeVotesCount { get; init; }
    public required int AwayVotesCount { get; init; }
    public required int DrawVotesCount { get; init; }

}
