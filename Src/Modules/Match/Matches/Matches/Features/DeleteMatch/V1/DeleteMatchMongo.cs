using Ardalis.GuardClauses;
using MapsterMapper;
using Match.Data;
using Match.Models;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Matchs.Exceptions;

namespace Match.Features.DeleteMatch.V1;

public record DeleteMatchMongo(Guid Id): InternalCommand;

public class DeleteMatchMongoHandler : ICommandHandler<DeleteMatchMongo>
{
    private readonly MatchReadDbContext _readDbContext;
    private readonly IMapper _mapper;

    public DeleteMatchMongoHandler(
        MatchReadDbContext readDbContext,
        IMapper mapper)
    {
        _readDbContext = readDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteMatchMongo request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var matchReadModel = _mapper.Map<MatchReadModel>(request);

        var @match = await _readDbContext.Match.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == matchReadModel.Id && !x.IsDeleted, cancellationToken);

        if (@match is null)
        {
            throw new MatchNotFoundException(request.Id);
        }

        await _readDbContext.Match.UpdateOneAsync(
            x => x.Id == matchReadModel.Id,
            Builders<MatchReadModel>.Update
                .Set(x => x.IsDeleted, matchReadModel.IsDeleted),
            cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
