using Mapster;

namespace Match.Features;

using Match.Features.CreateMatch.V1;
using Match.Features.UpdateMatch.V1;
using Match.Features.UpdateMatchStat.V1;
using Match.Features.GetMatches.V1;
using Match.Features.GetMatch.V1;
using Match.Features.DeleteMatch.V1;
using MassTransit;
using Models;
using MatchDto = Dtos.MatchDto;

public class MatchMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map domain model to DTO used by gRPC / API layers.
        config.NewConfig<Models.MatchModel, MatchDto>()
            .Map(d => d.Id, s => s.Id.Value.ToString())
            .Map(d => d.HomeTeam, s => s.HomeTeam)
            .Map(d => d.AwayTeam, s => s.AwayTeam)
            .Map(d => d.HomeTeamScore, s => s.HomeTeamScore)
            .Map(d => d.AwayTeamScore, s => s.AwayTeamScore)
            .Map(d => d.League, s => s.League)
            .Map(d => d.Status, s => s.Status)
            .Map(d => d.MatchTime, s => s.MatchTime)
            .Map(d => d.EventsCount, s => s.EventsCount)
            .Map(d => d.HomeVotesCount, s => s.HomeVotesCount)
            .Map(d => d.AwayVotesCount, s => s.AwayVotesCount)
            .Map(d => d.DrawVotesCount, s => s.DrawVotesCount);

        // Keep these CRUD/read-model mappings if your persistence models exist.
        // They are left intentionally minimal — adjust fields/names to match your read model types.
        config.NewConfig<CreateMatchMongo, MatchReadModel>()
            .Map(d => d.Id, s => NewId.NextGuid())
            .Map(d => d.MatchId, s => s.Id);

        config.NewConfig<Models.Match, MatchReadModel>()
            .Map(d => d.Id, s => NewId.NextGuid())
            .Map(d => d.MatchId, s => s.Id.Value);

        config.NewConfig<MatchReadModel, MatchDto>()
            .Map(d => d.Id, s => s.MatchId);

        config.NewConfig<UpdateMatchMongo, MatchReadModel>()
            .Map(d => d.MatchId, s => s.Id);

        config.NewConfig<DeleteMatchMongo, MatchReadModel>()
            .Map(d => d.MatchId, s => s.Id);

        config.NewConfig<CreateMatchRequestDto, CreateMatch>()
            .ConstructUsing(x => new CreateMatch(x.MatchNumber, x.AircraftId, x.DepartureAirportId,
                x.DepartureDate, x.ArriveDate, x.ArriveAirportId, x.DurationMinutes, x.MatchDate, x.Status, x.Price));

        config.NewConfig<UpdateMatchRequestDto, UpdateMatch>()
            .ConstructUsing(x => new UpdateMatch(x.Id, x.MatchNumber, x.AircraftId, x.DepartureAirportId, x.DepartureDate,
                x.ArriveDate, x.ArriveAirportId, x.DurationMinutes, x.MatchDate, x.Status, x.IsDeleted, x.Price));
    }
}