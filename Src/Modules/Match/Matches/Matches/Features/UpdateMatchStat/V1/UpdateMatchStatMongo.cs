using Ardalis.GuardClauses;
using MapsterMapper;
using Match.Data;
using Match.Matches.Models;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Matchs.Exceptions;

namespace Match.Matches.Features.UpdateMatchStat.V1;

public record UpdateMatchStatMongo(
    Guid MatchId,
    int HomeTeamScore,
    int AwayTeamScore) : InternalCommand;

public class UpdateMatchStatMongoHandler : ICommandHandler<UpdateMatchStatMongo>
{
    private readonly MatchReadDbContext _readDbContext;
    private readonly IMapper _mapper;

    public UpdateMatchStatMongoHandler(
        MatchReadDbContext readDbContext,
        IMapper mapper)
    {
        _readDbContext = readDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateMatchStatMongo request, CancellationToken cancellationToken)
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
                .Set(x => x.HomeTeamScore, matchReadModel.HomeTeamScore)
                .Set(x => x.AwayTeamScore, matchReadModel.AwayTeamScore),
            cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
