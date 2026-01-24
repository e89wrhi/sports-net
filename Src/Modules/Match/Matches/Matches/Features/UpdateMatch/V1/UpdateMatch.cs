using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using Matches.Data;
using Matches.Matches.Enums;
using Matches.Matches.Models;
using Matches.Matches.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Sport.Common.Core;
using Sport.Common.Web;
using Sport.Matchs.Exceptions;

namespace Matches.Matches.Features.UpdateMatch.V1;

public record UpdateMatchCommand(
    Guid MatchId,
    string HomeTeam,
    string AwayTeam,
    MatchLeague League,
    MatchStatus Status,
    DateTime MatchTime) : ICommand<UpdateMatchCommandResponse>
{
}

public record UpdateMatchCommandResponse(Guid Id);

public record UpdateMathRequest(
    Guid MatchId,
    string HomeTeam,
    string AwayTeam,
    MatchLeague League,
    MatchStatus Status,
    DateTime MatchTime);

public record UpdateMatchRequestResponse(Guid Id);

public record UpdateMatchRequest(
    );

public class UpdateMatchEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/matches", async (UpdateMatchRequest request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
        {
            var command = mapper.Map<UpdateMatchCommand>(request);

            var result = await mediator.Send(command, cancellationToken);

            var response = result.Adapt<UpdateMatchRequestResponse>();

            return Results.Ok(response);
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("UpdateMatch")
            .WithApiVersionSet(builder.NewApiVersionSet("Match").Build())
            .Produces<UpdateMatchRequestResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Match")
            .WithDescription("Update Match")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class UpdateMatchCommandValidator : AbstractValidator<UpdateMatchCommand>
{
    public UpdateMatchCommandValidator()
    {
        RuleFor(x => x.HomeTeam).NotEmpty().WithMessage("Home Team is required");
        RuleFor(x => x.AwayTeam).NotEmpty().WithMessage("Away Team is required");
        RuleFor(x => x.League).NotEmpty().WithMessage("League is required");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");
        RuleFor(x => x.MatchTime).NotEmpty().WithMessage("Match Time is required");
    }
}

internal class UpdateMatchHandler : IRequestHandler<UpdateMatchCommand, UpdateMatchCommandResponse>
{
    private readonly MatchDbContext _dbContext;

    public UpdateMatchHandler(MatchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UpdateMatchCommandResponse> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var item =
            await _dbContext.Matches.SingleOrDefaultAsync(x => x.Id.Value == request.MatchId, cancellationToken);

        if (item is null)
        {
            throw new MatchNotFoundException(request.MatchId);
        }

        item.Update(HomeTeam.Of(request.HomeTeam), AwayTeam.Of(request.AwayTeam),
            request.League, request.Status, request.MatchTime);
        _dbContext.Matches.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateMatchCommandResponse(item.Id);
    }
}

