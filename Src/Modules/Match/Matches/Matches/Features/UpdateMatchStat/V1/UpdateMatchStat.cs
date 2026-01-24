using Bogus.DataSets;
using FluentValidation;
using Google.Rpc;
using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Match.Data;
using Match.Matches.Enums;
using Match.Matches.Models;
using Match.Matches.ValueObjects;
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

namespace Match.Matches.Features.UpdateMatchStat.V1;

public record UpdateMatchStatCommand(
    Guid MatchId,
    int HomeTeamScore,
    int AwayTeamScore) : ICommand<UpdateMatchStatCommandResponse>
{
}

public record UpdateMatchStatCommandResponse(Guid Id);

public record UpdateMatchStatRequest(
    Guid MatchId,
    int HomeTeamScore,
    int AwayTeamScore);

public record UpdateMatchStatRequestResponse(Guid Id);

public class UpdateMatchStatEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/matches", async (UpdateMatchStatRequest request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
        {
            var command = mapper.Map<UpdateMatchStatCommand>(request);

            var result = await mediator.Send(command, cancellationToken);

            var response = result.Adapt<UpdateMatchStatRequestResponse>();

            return Results.Ok(response);
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("UpdateMatchStat")
            .WithApiVersionSet(builder.NewApiVersionSet("Match").Build())
            .Produces<UpdateMatchStatRequestResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Match Stat")
            .WithDescription("Update Match Stat")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class UpdateMatchStatCommandValidator : AbstractValidator<UpdateMatchStatCommand>
{
    public UpdateMatchStatCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty().WithMessage("Match is required");
        RuleFor(x => x.HomeTeamScore).NotEmpty().WithMessage("Home Team Score is required");
        RuleFor(x => x.AwayTeamScore).NotEmpty().WithMessage("Away Team Score is required");
    }
}

internal class UpdateMatchStatHandler : IRequestHandler<UpdateMatchStatCommand, UpdateMatchStatCommandResponse>
{
    private readonly MatchDbContext _dbContext;

    public UpdateMatchStatHandler(MatchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UpdateMatchStatCommandResponse> Handle(UpdateMatchStatCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var item =
            await _dbContext.Matches.SingleOrDefaultAsync(x => x.Id.Value == request.MatchId, cancellationToken);

        if (item is null)
        {
            throw new MatchNotFoundException(request.MatchId);
        }

        item.UpdateScore(Score.Of(request.HomeTeamScore), Score.Of(request.AwayTeamScore));
        _dbContext.Matches.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateMatchStatCommandResponse(item.Id);
    }
}

