using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Match.Data;
using Match.Enums;
using Match.Models;
using Match.ValueObjects;
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

namespace Match.Features.CreateMatch.V1;

public record CreateMatchCommand(
    HomeTeam HomeTeam,
    AwayTeam AwayTeam,
    MatchLeague League,
    MatchStatus Status,
    DateTime StartAt,
    DateTime FinishAt,
    string Referee) : ICommand<CreateMatchCommandResponse>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record CreateMatchCommandResponse(Guid Id, bool Success);

public record CreateMatchRequest(
    string HomeTeam,
    string AwayTeam,
    MatchLeague League,
    MatchStatus Status,
    DateTime StartAt,
    DateTime FinishAt,
    string Referee);

public record CreateMatchRequestResponse(Guid Id, bool Success);

public class CreateMatchEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/matches", async (CreateMatchRequest request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
        {
            var command = mapper.Map<CreateMatchCommand>(request);

            var result = await mediator.Send(command, cancellationToken);

            var response = result.Adapt<CreateMatchRequestResponse>();

            return Results.Ok(response);
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("CreateMatch")
            .WithApiVersionSet(builder.NewApiVersionSet("Match").Build())
            .Produces<CreateMatchRequestResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Match")
            .WithDescription("Create Match")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class CreateMatchCommandValidator : AbstractValidator<CreateMatchCommand>
{
    public CreateMatchCommandValidator()
    {
        RuleFor(x => x.HomeTeam).NotEmpty().WithMessage("Home Team is required");
        RuleFor(x => x.AwayTeam).NotEmpty().WithMessage("Away Team is required");
        RuleFor(x => x.League).NotEmpty().WithMessage("League is required");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");
        RuleFor(x => x.StartAt).NotEmpty().WithMessage("Match Time is required");
    }
}

internal class CreateMatchHandler : IRequestHandler<CreateMatchCommand, CreateMatchCommandResponse>
{
    private readonly MatchDbContext _dbContext;

    public CreateMatchHandler(MatchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateMatchCommandResponse> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var itemEntity = Models.MatchModel.Create(HomeTeam.Of(request.HomeTeam), AwayTeam.Of(request.AwayTeam),
            request.League, request.Status, request.StartAt, request.FinishAt, request.Referee);

        var newMatch = (await _dbContext.Matches.AddAsync(itemEntity, cancellationToken)).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new CreateMatchCommandResponse(newMatch.Id, true);
    }
}


