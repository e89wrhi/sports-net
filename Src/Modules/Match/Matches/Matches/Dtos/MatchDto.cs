using Match.Enums;

namespace Match.Dtos;

public record MatchDto(
    Guid Id,
    string HomeTeam,
    string AwayTeam,
    int HomeTeamScore,
    int AwayTeamScore,
    MatchLeague League,
    MatchStatus Status,
    DateTime MatchTime,
    int EventsCount,
    int HomeVotesCount,
    int AwayVotesCount,
    int DrawVotesCount);
