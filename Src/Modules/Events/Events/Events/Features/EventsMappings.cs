using Mapster;

namespace Events.Features;

using Events.Features.AddEvent.V1;
using Events.Features.DeleteEvent.V1;
using Events.Features.GetEvents.V1;
using MassTransit;
using Events;
using EventDto = Dtos.EventDto;
using Events.Models;
using Events.Enums;

public class EventMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map domain model to DTO used by gRPC / API layers.
        config.NewConfig<Models.EventModel, EventDto>()
            .Map(d => d.Id, s => s.Id.Value.ToString())
            .Map(d => d.MatchId, s => s.MatchId.Value.ToString())
            .Map(d => d.Title, s => s.Title.Value)
            .Map(d => d.Time, s => s.Time.Value)
            .Map(d => d.Type, s => s.Type.ToString());

        // Persistence/read-model mappings (adjust to your actual read model shapes).
        config.NewConfig<AddEventMongo, EventReadModel>()
            .Map(d => d.Id, s => NewId.NextGuid())
            .Map(d => d.Id, s => s.Id);

        config.NewConfig<Models.EventModel, EventReadModel>()
            .Map(d => d.Id, s => NewId.NextGuid())
            .Map(d => d.Id, s => s.Id.Value);

        config.NewConfig<EventReadModel, EventDto>()
            .Map(d => d.Id, s => s.Id);

        config.NewConfig<DeleteEventMongo, EventReadModel>()
            .Map(d => d.Id, s => s.Id);

        config.NewConfig<AddEventRequest, AddEventCommand>()
            .ConstructUsing(x => new AddEventCommand(x.MatchId, x.Title, x.Time,
                x.Type.ToString()));

        config.NewConfig<DeleteEventR, DeleteEventCommand>()
            .ConstructUsing(x => new UpdateEvent(x.Id, x.EventNumber, x.AircraftId, x.DepartureAirportId, x.DepartureDate,
                x.ArriveDate, x.ArriveAirportId, x.DurationMinutes, x.EventDate, x.Status, x.IsDeleted, x.Price));
    }
}