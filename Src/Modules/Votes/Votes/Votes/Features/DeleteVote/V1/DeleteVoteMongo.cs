using Ardalis.GuardClauses;
using MapsterMapper;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Votes.Exceptions;
using Vote.Data;
using Vote.Models;

namespace Vote.Features.DeleteVote.V1;

public record DeleteVoteMongo(Guid Id, Guid MatchId, Guid VoterId) : InternalCommand;

public class DeleteVoteMongoHandler : ICommandHandler<DeleteVoteMongo>
{
    private readonly VoteReadDbContext _readDbContext;
    private readonly IMapper _mapper;

    public DeleteVoteMongoHandler(
        VoteReadDbContext readDbContext,
        IMapper mapper)
    {
        _readDbContext = readDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteVoteMongo request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var voteReadModel = _mapper.Map<VoteReadModel>(request);

        var @vote = await _readDbContext.Vote.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == voteReadModel.Id, cancellationToken);

        if (@vote is null)
        {
            throw new VoteNotFoundException(request.Id);
        }

        await _readDbContext.Vote.DeleteOneAsync(x => x.Id == voteReadModel.Id, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
