using Sport.Common.Core;

namespace Intelligence.Features.PredictMatch.V1;

public record PredictMatchCommand(
    string HomeTeam,
    string AwayTeam,
    int HomeScore,
    int AwayScore,
    int Minute,
    IReadOnlyList<MatchEventDto> Events,
    TeamInfoDto HomeTeamInfo,
    TeamInfoDto AwayTeamInfo,
    VoteStatsDto Votes, 
    string? ModelId = null) : ICommand<PredictMatchCommandResult>;

public sealed record MatchEventDto(
    int Minute,
    string Team,
    string Type); // Goal, RedCard, YellowCard, Injury, Substitution

public sealed record TeamInfoDto(
    string Name,
    int Ranking,
    string Form,        // e.g. "WWDLW"
    bool IsHome);

public sealed record VoteStatsDto(
    int HomeWinVotes,
    int DrawVotes,
    int AwayWinVotes);

public record PredictMatchCommandResult(string Prediction,
    double HomeWinProbability,
    double DrawProbability,
    double AwayWinProbability, 
    string ModelId, string? ProviderName);
