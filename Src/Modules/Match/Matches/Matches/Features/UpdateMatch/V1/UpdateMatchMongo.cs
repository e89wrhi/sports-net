using Ardalis.GuardClauses;
using MapsterMapper;
using Match.Data;
using Match.Matches.Models;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Matchs.Exceptions;

namespace Match.Matches.Features.UpdateMatch.V1;

public record UpdateMatchMongo(
    Guid MatchId,
    string HomeTeam,
    string AwayTeam,
    string League,
    string Status,
    DateTime MatchTime) : InternalCommand;


public class UpdateMatchMongoHandler : ICommandHandler<UpdateMatchMongo>
{
    private readonly MatchReadDbContext _readDbContext;
    private readonly IMapper _mapper;

    public UpdateMatchMongoHandler(
        MatchReadDbContext readDbContext,
        IMapper mapper)
    {
        _readDbContext = readDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateMatchMongo request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var matchReadModel = _mapper.Map<MatchReadModel>(request);

        var @match = await _readDbContext.Match.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == matchReadModel.Id && !x.IsDeleted, cancellationToken);

        if (@match is null)
        {
            throw new MatchNotFoundException(request.MatchId);
        }

        await _readDbContext.Match.UpdateOneAsync(
            x => x.Id == matchReadModel.Id,
            Builders<MatchReadModel>.Update
                .Set(x => x.HomeTeam, matchReadModel.HomeTeam)
                .Set(x => x.AwayTeam, matchReadModel.AwayTeam)
                .Set(x => x.League, matchReadModel.League)
                .Set(x => x.Status, matchReadModel.Status)
                .Set(x => x.MatchTime, matchReadModel.MatchTime),
            cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
