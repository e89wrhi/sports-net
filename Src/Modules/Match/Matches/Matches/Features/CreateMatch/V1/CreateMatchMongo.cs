using Ardalis.GuardClauses;
using MapsterMapper;
using Match.Data;
using Match.Matches.Models;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Matchs.Exceptions;

namespace Match.Matches.Features.CreateMatch.V1;

public record CreateMatchMongo(
    Guid Id,
    string HomeTeam,
    string AwayTeam,
    string League,
    string Status,
    DateTime MatchTime) : InternalCommand;

public class CreateMatchMongoHandler : ICommandHandler<CreateMatchMongo>
{
    private readonly MatchReadDbContext _readDbContext;
    private readonly IMapper _mapper;

    public CreateMatchMongoHandler(
        MatchReadDbContext readDbContext,
        IMapper mapper)
    {
        _readDbContext = readDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateMatchMongo request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var matchReadModel = _mapper.Map<MatchReadModel>(request);

        var @match = await _readDbContext.Match.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == matchReadModel.Id && !x.IsDeleted, cancellationToken);

        if (@match is not null)
        {
            throw new MatchAlreadyExistException(request.Id);
        }

        await _readDbContext.Match.InsertOneAsync(matchReadModel, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
