namespace Event.Data.Seed;

using System;
using System.Collections.Generic;
using System.Linq;
using global::Events.Models;
using MassTransit;

public static class InitialData
{
    public static List<EventModel> Events { get; }


    static InitialData()
    {
        var elClasicoId = MatchId.Of(Guid.Parse("4e6cbdd8-ebc9-4619-aba6-f3517f412137"));

        Events = new List<EventModel>
        {
            EventModel.Create(
                elClasicoId,
                Title.Of("Goal - Vinícius Júnior"),
                Time.Of("15'"),
                EventType.Goal),

            EventModel.Create(
                elClasicoId,
                Title.Of("Yellow Card - Gavi"),
                Time.Of("30'"),
                EventType.YellowCard),

            EventModel.Create(
                elClasicoId,
                Title.Of("Goal - Robert Lewandowski"),
                Time.Of("55'"),
                EventType.Goal),
                
            EventModel.Create(
                MatchId.Of(Guid.Parse("6a173823-1cd2-4087-bde0-737dfe90cd86")),
                Title.Of("Goal - Harry Kane"),
                Time.Of("12'"),
                EventType.Goal),

            EventModel.Create(
                MatchId.Of(Guid.Parse("6a173823-1cd2-4087-bde0-737dfe90cd86")),
                Title.Of("Substitution - Müller for Musiala"),
                Time.Of("75'"),
                EventType.Substitution),

            // More Events for El Clasico
            EventModel.Create(
                elClasicoId,
                Title.Of("Red Card - Dani Carvajal"),
                Time.Of("82'"),
                EventType.RedCard),

            EventModel.Create(
                elClasicoId,
                Title.Of("Penalty - Barcelona"),
                Time.Of("83'"),
                EventType.Penality),

            // Events for Leverkusen vs Leipzig (Live)
            EventModel.Create(
                MatchId.Of(Guid.Parse("b139c677-1cd5-4e41-850c-0a686baa901e")),
                Title.Of("Goal - Florian Wirtz"),
                Time.Of("10'"),
                EventType.Goal),

            EventModel.Create(
                MatchId.Of(Guid.Parse("b139c677-1cd5-4e41-850c-0a686baa901e")),
                Title.Of("Yellow Card - Xavi Simons"),
                Time.Of("25'"),
                EventType.YellowCard),

            EventModel.Create(
                MatchId.Of(Guid.Parse("b139c677-1cd5-4e41-850c-0a686baa901e")),
                Title.Of("Goal - Loïs Openda"),
                Time.Of("41'"),
                EventType.Goal),

            // Events for Atletico vs Sevilla (Over)
            EventModel.Create(
                MatchId.Of(Guid.Parse("ca7a5bf2-8d3c-47fa-970f-e52915607300")),
                Title.Of("Goal - Antoine Griezmann"),
                Time.Of("33'"),
                EventType.Goal),

            EventModel.Create(
                MatchId.Of(Guid.Parse("ca7a5bf2-8d3c-47fa-970f-e52915607300")),
                Title.Of("Foul - Marcos Acuña"),
                Time.Of("45+2'"),
                EventType.Foul)
        };
    }
}