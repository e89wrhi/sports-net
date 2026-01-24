using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Match.Data;
using Match.Matches.Enums;
using Match.Matches.Models;
using Match.Matches.ValueObjects;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sport.Common.Core;
using Sport.Common.Web;
using Microsoft.EntityFrameworkCore;
using Sport.Matchs.Exceptions;

namespace Match.Matches.Features.DeleteMatch.V1;

public record DeleteMatchCommand(
    Guid MatchId) : ICommand<DeleteMatchCommandResponse>
{
}

public record DeleteMatchCommandResponse(Guid Id);

public class DeleteMatchEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {

        builder.MapDelete($"{EndpointConfig.BaseApiPath}/matches/{{id}}", async (Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new DeleteMatchCommand(id), cancellationToken);

            return Results.NoContent();
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("DeleteMatch")
            .WithApiVersionSet(builder.NewApiVersionSet("Match").Build())
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Match")
            .WithDescription("Delete Match")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class DeleteMatchCommandValidator : AbstractValidator<DeleteMatchCommand>
{
    public DeleteMatchCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty().WithMessage("Match is required");
    }
}

internal class DeleteMatchHandler : IRequestHandler<DeleteMatchCommand, DeleteMatchCommandResponse>
{
    private readonly MatchDbContext _dbContext;

    public DeleteMatchHandler(MatchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeleteMatchCommandResponse> Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var item =
            await _dbContext.Matches.SingleOrDefaultAsync(x => x.Id == MatchId.Of(request.MatchId), cancellationToken);

        if (item is null)
        {
            throw new MatchNotFoundException(request.MatchId);
        }

        _dbContext.Matches.Remove(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteMatchCommandResponse(request.MatchId);
    }
}

