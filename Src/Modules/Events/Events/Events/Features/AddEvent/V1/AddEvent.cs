namespace Events.Events.Features.AddEvent.V1;

using Aircrafts.ValueObjects;
using Ardalis.GuardClauses;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Web;
using Data;
using Duende.IdentityServer.EntityFramework.Entities;
using Exceptions;
using Event.Airports.ValueObjects;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Sport.Common.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using ValueObjects;

public record AddEvent(string name) : ICommand<AddEventResult>, IInternalCommand
{

}

public record AddEventResult();

public record EventCreatedDomainEvent() : IDomainEvent;

public record AddEventRequestDto();

public record AddEventResponseDto();

public class AddEventEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/event", async (CreateEventRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
        {
            var command = mapper.Map<CreateEvent>(request);

            var result = await mediator.Send(command, cancellationToken);

            var response = result.Adapt<CreateEventResponseDto>();

            return Results.CreatedAtRoute("GetEvents", new { id = result.Id }, response);
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("CreateEvent")
            .WithApiVersionSet(builder.NewApiVersionSet("Event").Build())
            .Produces<CreateEventResponseDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Event")
            .WithDescription("Create Event")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class AddEventValidator : AbstractValidator<AddEvent>
{

}

internal class AddEventHandler : ICommandHandler<CreateEvent, CreateEventResult>
{
    private readonly EventDbContext _eventDbContext;

    public CreateEventHandler(EventDbContext eventDbContext)
    {
        _eventDbContext = eventDbContext;
    }

    public async Task<CreateEventResult> Handle(CreateEvent request, CancellationToken cancellationToken)
    {
    }
}