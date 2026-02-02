using System;
using System.Collections.Generic;
using Events.Models;
using Events.ValueObjects;
using Events.Enums;

namespace Events.Data.Seed;

public static class InitialData
{
    public static List<EventModel> Events { get; }


    static InitialData()
    {
        DateTime baseMatchTime = DateTime.UtcNow.Date.AddHours(19); // base time for seeded matches

        static Time ToTime(string s, DateTime baseTime)
        {
            var clean = s.Trim();
            if (clean.EndsWith("'")) clean = clean[..^1];

            int minutes;
            if (clean.Contains('+'))
            {
                var parts = clean.Split('+', StringSplitOptions.RemoveEmptyEntries);
                minutes = int.Parse(parts[0]) + int.Parse(parts[1]);
            }
            else
            {
                minutes = int.Parse(clean);
            }

            return Time.Of(baseTime.AddMinutes(minutes));
        }

        var elClasicoId = MatchId.Of(Guid.Parse("4e6cbdd8-ebc9-4619-aba6-f3517f412137"));

        Events = new List<EventModel>
        {
            EventModel.Create(
                elClasicoId,
                Title.Of("Goal - Vinícius Júnior"),
                ToTime("15'", baseMatchTime),
                EventType.Goal),

            EventModel.Create(
                elClasicoId,
                Title.Of("Yellow Card - Gavi"),
                ToTime("30'", baseMatchTime),
                EventType.YellowCard),

            EventModel.Create(
                elClasicoId,
                Title.Of("Goal - Robert Lewandowski"),
                ToTime("55'", baseMatchTime),
                EventType.Goal),
                
            EventModel.Create(
                MatchId.Of(Guid.Parse("6a173823-1cd2-4087-bde0-737dfe90cd86")),
                Title.Of("Goal - Harry Kane"),
                ToTime("12'", baseMatchTime),
                EventType.Goal),

            EventModel.Create(
                MatchId.Of(Guid.Parse("6a173823-1cd2-4087-bde0-737dfe90cd86")),
                Title.Of("Substitution - Müller for Musiala"),
                ToTime("75'", baseMatchTime),
                EventType.Substitution),

            // More Events for El Clasico
            EventModel.Create(
                elClasicoId,
                Title.Of("Red Card - Dani Carvajal"),
                ToTime("82'", baseMatchTime),
                EventType.RedCard),

            EventModel.Create(
                elClasicoId,
                Title.Of("Penalty - Barcelona"),
                ToTime("83'", baseMatchTime),
                EventType.Penality),

            // Events for Leverkusen vs Leipzig (Live)
            EventModel.Create(
                MatchId.Of(Guid.Parse("b139c677-1cd5-4e41-850c-0a686baa901e")),
                Title.Of("Goal - Florian Wirtz"),
                ToTime("10'", baseMatchTime),
                EventType.Goal),

            EventModel.Create(
                MatchId.Of(Guid.Parse("b139c677-1cd5-4e41-850c-0a686baa901e")),
                Title.Of("Yellow Card - Xavi Simons"),
                ToTime("25'", baseMatchTime),
                EventType.YellowCard),

            EventModel.Create(
                MatchId.Of(Guid.Parse("b139c677-1cd5-4e41-850c-0a686baa901e")),
                Title.Of("Goal - Loïs Openda"),
                ToTime("41'", baseMatchTime),
                EventType.Goal),

            // Events for Atletico vs Sevilla (Over)
            EventModel.Create(
                MatchId.Of(Guid.Parse("ca7a5bf2-8d3c-47fa-970f-e52915607300")),
                Title.Of("Goal - Antoine Griezmann"),
                ToTime("33'", baseMatchTime),
                EventType.Goal),

            EventModel.Create(
                MatchId.Of(Guid.Parse("ca7a5bf2-8d3c-47fa-970f-e52915607300")),
                Title.Of("Foul - Marcos Acuña"),
                ToTime("45+2'", baseMatchTime),
                EventType.Foul)
        };
    }
}