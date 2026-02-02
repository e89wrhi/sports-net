using Mapster;

namespace Vote.Features;

using Vote.Features.AddVote.V1;
using Vote.Features.DeleteVote.V1;
using MassTransit;
using Models;

public class VoteMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Persistence/read-model mappings (adjust to your actual read model shapes).
        config.NewConfig<CreateVoteMongo, VoteReadModel>()
            .Map(d => d.Id, s => NewId.NextGuid())
            .Map(d => d.VoteId, s => s.Id);

        config.NewConfig<Models.Vote, VoteReadModel>()
            .Map(d => d.Id, s => NewId.NextGuid())
            .Map(d => d.VoteId, s => s.Id.Value);

        config.NewConfig<VoteReadModel, VoteDto>()
            .Map(d => d.Id, s => s.VoteId);

        config.NewConfig<UpdateVoteMongo, VoteReadModel>()
            .Map(d => d.VoteId, s => s.Id);

        config.NewConfig<DeleteVoteMongo, VoteReadModel>()
            .Map(d => d.VoteId, s => s.Id);

        config.NewConfig<CreateVoteRequestDto, CreateVote>()
            .ConstructUsing(x => new CreateVote(x.VoteNumber, x.AircraftId, x.DepartureAirportId,
                x.DepartureDate, x.ArriveDate, x.ArriveAirportId, x.DurationMinutes, x.VoteDate, x.Status, x.Price));

        config.NewConfig<UpdateVoteRequestDto, UpdateVote>()
            .ConstructUsing(x => new UpdateVote(x.Id, x.VoteNumber, x.AircraftId, x.DepartureAirportId, x.DepartureDate,
                x.ArriveDate, x.ArriveAirportId, x.DurationMinutes, x.VoteDate, x.Status, x.IsDeleted, x.Price));
    }
}