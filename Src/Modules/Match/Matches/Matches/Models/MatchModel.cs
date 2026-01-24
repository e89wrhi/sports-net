using Matches.Matches.Enums;
using Matches.Matches.ValueObjects;
using Sport.Common.Core;

namespace Matches.Matches.Models;

public record MatchModel : Aggregate<MatchId>
{
    public HomeTeam HomeTeam { get; private set; } = default!;
    public AwayTeam AwayTeam { get; private set; } = default!;
    public Score HomeTeamScore { get; private set; } = default!;
    public Score AwayTeamScore { get; private set; } = default!;
    public MatchLeague League { get; private set; } = default!;
    public MatchStatus Status { get; private set; } = default!;
    public DateTime MatchTime { get; private set; } = default!;

    public int EventsCount { get; private set; } = default!;
    public int HomeVotesCount { get; private set; } = default!;
    public int AwayVotesCount { get; private set; } = default!;
    public int DrawVotesCount { get; private set; } = default!;

    public static MatchModel Create(HomeTeam homeTeam, AwayTeam awayTeam,
          MatchLeague matchLeague, MatchStatus matchStatus, DateTime matchTime)
    {
        var item = new MatchModel()
        {
            Id = MatchId.Of(Guid.NewGuid()),
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            League = matchLeague,
            Status = matchStatus,
            MatchTime = matchTime,
            Version = 1,
        };

        var @event = new MatchCreatedDomainEvent(item.Id);
        item.AddDomainEvent(@event);
        return item;
    }

    public void Update(HomeTeam homeTeam, AwayTeam awayTeam,
          MatchLeague matchLeague, MatchStatus matchStatus, DateTime matchTime)
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        League = matchLeague;
        Status = matchStatus;
        MatchTime = matchTime;
        Version++;
        LastModified = DateTime.UtcNow;
        AddDomainEvent(new MatchUpdatedDomainEvent(Id));
    }

    public void UpdateScore(Score homeTeamScore, Score awayTeamScore)
    {
        HomeTeamScore = homeTeamScore;
        AwayTeamScore = awayTeamScore;
        Version++;
        LastModified = DateTime.UtcNow;
        AddDomainEvent(new MatchScoreUpdatedDomainEvent(Id));
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

public record MatchCreatedDomainEvent(MatchId Id) : IDomainEvent;
public record MatchUpdatedDomainEvent(MatchId Id) : IDomainEvent;
public record MatchScoreUpdatedDomainEvent(MatchId Id) : IDomainEvent;
public record MatchDeletedDomainEvent(MatchId Id) : IDomainEvent;
