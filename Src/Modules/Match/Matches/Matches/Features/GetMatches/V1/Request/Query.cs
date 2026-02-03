using Match.Dtos;
using Match.Enums;
using Sport.Common.Caching;
using Sport.Common.Core;

namespace Match.Features.GetMatches.V1;

public record GetMatchs : IQuery<GetMatchsResult>, ICacheRequest
{
    public MatchLeague League;
    public MatchStatus Status;
    public string CacheKey => "GetMatchs";
    public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddHours(1);
}

public record GetMatchsResult(IEnumerable<MatchDto> MatchDtos);

public record GetMatchsResponseDto(IEnumerable<MatchDto> MatchDtos);

