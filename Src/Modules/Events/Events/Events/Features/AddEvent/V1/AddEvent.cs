using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Event.Data;
using Events.Enums;
using Events.Models;
using Events.ValueObjects;
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
using Sport.Common.Web;

namespace Events.Features.AddEvent.V1;

public record AddEventCommand(
    Guid MatchId,
    string Title,
    DateTime Time,
    string Type) : ICommand<AddEventCommandResponse>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record AddEventCommandResponse(Guid Id);

public record AddEventRequest(
    Guid MatchId,
    string Title,
    DateTime Time,
    Enums.EventType Type);

public record AddEventRequestResponse(Guid Id);

public class AddEventEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/events", async (AddEventRequest request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
        {
            var command = mapper.Map<AddEventCommand>(request);

            var result = await mediator.Send(command, cancellationToken);

            var response = result.Adapt<AddEventRequestResponse>();

            return Results.Ok(response);
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("AddEvent")
            .WithApiVersionSet(builder.NewApiVersionSet("Event").Build())
            .Produces<AddEventRequestResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Event")
            .WithDescription("Add Event")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
{
    public AddEventCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty().WithMessage("Match is required");
        RuleFor(x => x.Time).NotEmpty().WithMessage("Time is required");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
    }
}

internal class AddEventHandler : IRequestHandler<AddEventCommand, AddEventCommandResponse>
{
    private readonly EventDbContext _dbContext;

    public AddEventHandler(EventDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AddEventCommandResponse> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var itemEntity = Models.EventModel.Create(MatchId.Of(request.MatchId),
            Title.Of(request.Title), Time.Of(request.Time), request.Type);

        var newEvent = (await _dbContext.Events.AddAsync(itemEntity, cancellationToken)).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new AddEventCommandResponse(newEvent.Id);
    }
}
