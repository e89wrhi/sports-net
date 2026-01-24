using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Votes.Data;
using Votes.Votes.Enums;
using Votes.Votes.Models;
using Votes.Votes.ValueObjects;
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

namespace Votes.Votes.Features.DeleteVote.V1;

public record DeleteVoteCommand(
    Guid MatchId,
    Guid VoterId,
    VoteType VoteType) : ICommand<DeleteVoteCommandResponse>
{
}

public record DeleteVoteCommandResponse(Guid Id);

public record DeleteVoteRequest(
    Guid MatchId,
    Guid VoterId,
    VoteType VoteType);

public record DeleteVoteRequestResponse(Guid Id);

public class DeleteVoteEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/votes/{{id}}", async (DeleteVoteRequest request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
        {
            var command = mapper.Map<DeleteVoteCommand>(request);

            var result = await mediator.Send(command, cancellationToken);

            var response = result.Adapt<DeleteVoteRequestResponse>();

            return Results.Ok(response);
        })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("DeleteVote")
            .WithApiVersionSet(builder.NewApiVersionSet("Vote").Build())
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Vote")
            .WithDescription("Delete Vote")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class DeleteVoteCommandValidator : AbstractValidator<DeleteVoteCommand>
{
    public DeleteVoteCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty().WithMessage("Code is required");
        RuleFor(x => x.VoterId).NotEmpty().WithMessage("Name is required");
    }
}

internal class DeleteVoteHandler : IRequestHandler<DeleteVoteCommand, DeleteVoteCommandResponse>
{
    private readonly VoteDbContext _dbContext;

    public DeleteVoteHandler(VoteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeleteVoteCommandResponse> Handle(DeleteVoteCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var item =
            await _dbContext.Votes.SingleOrDefaultAsync(x => x.MatchId == MatchId.Of(request.MatchId)
            && x.VoterId == VoterId.Of(request.VoterId), cancellationToken);

        if (item is null)
        {
            throw new VoteNotFoundException(request.VoterId);
        }
        _dbContext.Votes.Remove(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteVoteCommandResponse(item.Id);
    }
}

