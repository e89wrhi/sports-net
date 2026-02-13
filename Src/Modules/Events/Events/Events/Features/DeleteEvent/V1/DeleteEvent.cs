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
using Sport.Events.Exceptions;

namespace Events.Features.DeleteEvent.V1;

public record DeleteEventCommand(
    Guid EventId) : ICommand<DeleteEventCommandResponse>
{
}

public record DeleteEventCommandResponse(Guid Id, bool Success);

public class DeleteEventEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {

        builder.MapDelete($"{EndpointConfig.BaseApiPath}/events/{{id}}", async (Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new DeleteEventCommand(id), cancellationToken);

            return Results.NoContent();
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("DeleteEvent")
            .WithApiVersionSet(builder.NewApiVersionSet("Event").Build())
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Event")
            .WithDescription("Delete Event")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage("Event is required");
    }
}

internal class DeleteEventHandler : IRequestHandler<DeleteEventCommand, DeleteEventCommandResponse>
{
    private readonly EventDbContext _dbContext;

    public DeleteEventHandler(EventDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeleteEventCommandResponse> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var item =
            await _dbContext.Events.SingleOrDefaultAsync(x => x.Id == EventId.Of(request.EventId), cancellationToken);

        if (item is null)
        {
            throw new EventNotFoundException(request.EventId);
        }

        _dbContext.Events.Remove(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteEventCommandResponse(request.EventId, true);
    }
}

