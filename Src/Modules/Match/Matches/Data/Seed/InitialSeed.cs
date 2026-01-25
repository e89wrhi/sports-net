namespace Match.Data.Seed;

using global::Match.Matches.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public static class InitialData
{
    public static List<MatchModel> Matchs { get; }


    static InitialData()
    {
        Matchs = new List<MatchModel>
        {
            MatchModel.Create(
                HomeTeam.Of("Arsenal"),
                AwayTeam.Of("Man City"),
                MatchLeague.PremierLeague,
                MatchStatus.Upcoming,
                DateTime.UtcNow.AddDays(1)) with { Id = MatchId.Of(Guid.Parse("8f031e7a-f2d7-4c58-bc9b-bbb107ac145f")) },

            MatchModel.Create(
                HomeTeam.Of("Real Madrid"),
                AwayTeam.Of("Barcelona"),
                MatchLeague.LaLiga,
                MatchStatus.Live,
                DateTime.UtcNow.AddHours(-1)) with { Id = MatchId.Of(Guid.Parse("4e6cbdd8-ebc9-4619-aba6-f3517f412137")) },

            MatchModel.Create(
                HomeTeam.Of("Bayern Munich"),
                AwayTeam.Of("Dortmund"),
                MatchLeague.Bundesliga,
                MatchStatus.Over,
                DateTime.UtcNow.AddDays(-1)) with { Id = MatchId.Of(Guid.Parse("6a173823-1cd2-4087-bde0-737dfe90cd86")) },

            MatchModel.Create(
                HomeTeam.Of("Liverpool"),
                AwayTeam.Of("Chelsea"),
                MatchLeague.PremierLeague,
                MatchStatus.Upcoming,
                DateTime.UtcNow.AddDays(2)) with { Id = MatchId.Of(Guid.Parse("7a250d3c-fe90-4d90-a971-ce2c658aba26")) },

            MatchModel.Create(
                HomeTeam.Of("Atletico Madrid"),
                AwayTeam.Of("Sevilla"),
                MatchLeague.LaLiga,
                MatchStatus.Over,
                DateTime.UtcNow.AddDays(-2)) with { Id = MatchId.Of(Guid.Parse("ca7a5bf2-8d3c-47fa-970f-e52915607300")) },

            MatchModel.Create(
                HomeTeam.Of("Bayer Leverkusen"),
                AwayTeam.Of("RB Leipzig"),
                MatchLeague.Bundesliga,
                MatchStatus.Live,
                DateTime.UtcNow.AddMinutes(-45)) with { Id = MatchId.Of(Guid.Parse("b139c677-1cd5-4e41-850c-0a686baa901e")) }
        };
    }
}