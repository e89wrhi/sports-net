using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Vote.Data;
using Vote.Enums;
using Vote.Models;
using Vote.ValueObjects;
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
using Sport.Votes.Exceptions;
using System.Security.Claims;

namespace Vote.Features.AddVote.V1;

public record AddVoteCommand(
    Guid MatchId,
    Guid VoterId,
    VoteType Type) : ICommand<AddVoteCommandResponse>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record AddVoteCommandResponse(Guid Id);

public record AddVoteRequest(
    Guid MatchId,
    Guid VoterId,
    VoteType Type);

public record AddVoteRequestResponse(Guid Id);

public class AddVoteEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/votes", async (AddVoteRequest request,
                IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor,
                CancellationToken cancellationToken) =>
        {
            // current user id
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var command = mapper.Map<AddVoteCommand>(request);

            var result = await mediator.Send(command, cancellationToken);

            var response = result.Adapt<AddVoteRequestResponse>();

            return Results.Ok(response);
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("AddVote")
            .WithApiVersionSet(builder.NewApiVersionSet("Vote").Build())
            .Produces<AddVoteRequestResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Vote")
            .WithDescription("Add Vote")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class AddVoteCommandValidator : AbstractValidator<AddVoteCommand>
{
    public AddVoteCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty().WithMessage("Match is required");
        RuleFor(x => x.VoterId).NotEmpty().WithMessage("Voter is required");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
    }
}

internal class AddVoteHandler : IRequestHandler<AddVoteCommand, AddVoteCommandResponse>
{
    private readonly VoteDbContext _dbContext;

    public AddVoteHandler(VoteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AddVoteCommandResponse> Handle(AddVoteCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var item =
            await _dbContext.Votes.SingleOrDefaultAsync(x => x.MatchId == MatchId.Of(request.MatchId)
            && x.VoterId == VoterId.Of(request.VoterId), cancellationToken);

        if (item is not null)
        {
            throw new VoteAlreadyExistException(request.VoterId);
        }

        var itemEntity = Models.VoteModel.Create(VoterId.Of(request.VoterId),
            MatchId.Of(request.MatchId), request.Type, DateTime.UtcNow);

        var newVote = (await _dbContext.Votes.AddAsync(itemEntity, cancellationToken)).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new AddVoteCommandResponse(newVote.Id);
    }
}

