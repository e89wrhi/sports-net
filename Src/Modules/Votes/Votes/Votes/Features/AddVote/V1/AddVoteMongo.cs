using Ardalis.GuardClauses;
using MapsterMapper;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Votes.Exceptions;
using Vote.Data;
using Vote.Models;

namespace Vote.Features.AddVote.V1;

public record AddVoteMongo(
    Guid Id,
    Guid MatchId,
    Guid VoterId,
    string Type) : InternalCommand;

public class AddVoteMongoHandler : ICommandHandler<AddVoteMongo>
{
    private readonly VoteReadDbContext _readDbContext;
    private readonly IMapper _mapper;

    public AddVoteMongoHandler(
        VoteReadDbContext readDbContext,
        IMapper mapper)
    {
        _readDbContext = readDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddVoteMongo request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var voteReadModel = _mapper.Map<VoteReadModel>(request);

        var @vote = await _readDbContext.Vote.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == voteReadModel.Id, cancellationToken);

        if (@vote is not null)
        {
            throw new VoteAlreadyExistException(request.Id);
        }

        await _readDbContext.Vote.InsertOneAsync(voteReadModel, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
