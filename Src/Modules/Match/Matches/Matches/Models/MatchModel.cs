using Match.Enums;
using Match.ValueObjects;
using Sport.Common.Core;

namespace Match.Models;

public record MatchModel : Aggregate<MatchId>
{
    public HomeTeam HomeTeam { get; private set; } = default!;
    public AwayTeam AwayTeam { get; private set; } = default!;
    public Score HomeTeamScore { get; private set; } = default!;
    public Score AwayTeamScore { get; private set; } = default!;
    public MatchLeague League { get; private set; } = default!;
    public MatchStatus Status { get; private set; } = default!;
    public DateTime StartAt { get; private set; } = default!;
    public DateTime FinishAt { get; private set; } = default!;
    public string Referee { get; private set; } = default!;

    public int EventsCount { get; private set; } = default!;
    public int HomeVotesCount { get; private set; } = default!;
    public int AwayVotesCount { get; private set; } = default!;
    public int DrawVotesCount { get; private set; } = default!;

    public static MatchModel Create(HomeTeam homeTeam, AwayTeam awayTeam,
          MatchLeague matchLeague, MatchStatus matchStatus, 
          DateTime startAt, DateTime finishAt, string referee)
    {
        var item = new MatchModel()
        {
            Id = MatchId.Of(Guid.NewGuid()),
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            League = matchLeague,
            Status = matchStatus,
            StartAt = startAt,
            FinishAt = finishAt,
            Referee = referee,
            Version = 1,
        };

        var @event = new MatchCreatedDomainEvent(item.Id, homeTeam, awayTeam, matchLeague,
            matchStatus, startAt);
        item.AddDomainEvent(@event);
        return item;
    }

    public void Update(HomeTeam homeTeam, AwayTeam awayTeam,
          MatchLeague matchLeague, MatchStatus matchStatus, 
          DateTime startAt, DateTime finishAt, string referee)
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        League = matchLeague;
        Status = matchStatus;
        StartAt = startAt;
        FinishAt = finishAt;
        Referee = referee;
        Version++;
        LastModified = DateTime.UtcNow;
        AddDomainEvent(new MatchUpdatedDomainEvent(Id, homeTeam, awayTeam, matchLeague, matchStatus,
            startAt));
    }

    public void UpdateScore(Score homeTeamScore, Score awayTeamScore)
    {
        HomeTeamScore = homeTeamScore;
        AwayTeamScore = awayTeamScore;
        Version++;
        LastModified = DateTime.UtcNow;
        AddDomainEvent(new MatchScoreUpdatedDomainEvent(Id, homeTeamScore, awayTeamScore));
    }

    public void UpdateVotesCount(int homeVotesCount, int awayVotesCount, int drawVotesCount)
    {
        HomeVotesCount = homeVotesCount;
        AwayVotesCount = awayVotesCount;
        DrawVotesCount = drawVotesCount;
    }

    public void UpdateEventsCount(int count)
    {
        EventsCount = count;
    }

    public void Delete()
    {
        IsDeleted = true;
        LastModified = DateTime.UtcNow;
        AddDomainEvent(new MatchDeletedDomainEvent(Id));
    }
}

public record MatchCreatedDomainEvent(MatchId Id, HomeTeam HomeTeam, AwayTeam AwayTeam,
    MatchLeague League, MatchStatus Status, DateTime MatchTime) : IDomainEvent;
public record MatchUpdatedDomainEvent(MatchId Id, HomeTeam HomeTeam, AwayTeam AwayTeam,
    MatchLeague League, MatchStatus Status, DateTime MatchTime) : IDomainEvent;
public record MatchScoreUpdatedDomainEvent(MatchId Id, Score HomeTeamScore, Score AwayTeamScore) : IDomainEvent;
public record MatchDeletedDomainEvent(MatchId Id) : IDomainEvent;
