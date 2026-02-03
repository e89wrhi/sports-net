using Ardalis.GuardClauses;
using MapsterMapper;
using Match.Data;
using Match.Dtos;
using Microsoft.EntityFrameworkCore;
using Sport.Common.Core;
using Sport.Matchs.Exceptions;

namespace Match.Features.GetMatches.V1;

internal class GetMatchsHandler : IQueryHandler<GetMatchs, GetMatchsResult>
{
    private readonly IMapper _mapper;
    private readonly MatchDbContext _matchDbContext;

    public GetMatchsHandler(IMapper mapper, MatchDbContext matchDbContext)
    {
        _mapper = mapper;
        _matchDbContext = matchDbContext;
    }

    public async Task<GetMatchsResult> Handle(GetMatchs request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var match = (await _matchDbContext.Matches.AsQueryable().ToListAsync(cancellationToken))
            .Where(x => !x.IsDeleted);

        if (!match.Any())
        {
            throw new MatchNotFoundException(Guid.Empty);
        }

        var matchDtos = _mapper.Map<IEnumerable<MatchDto>>(match);

        return new GetMatchsResult(matchDtos);
    }
}

