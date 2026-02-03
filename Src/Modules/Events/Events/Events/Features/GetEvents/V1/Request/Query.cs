using Events.Dtos;
using Sport.Common.Caching;
using Sport.Common.Core;

namespace Events.Features.GetEvents.V1;

public record GetEvents : IQuery<GetEventsResult>, ICacheRequest
{
    public Guid MatchId { get; set; }
    public string CacheKey => "GetEvents";
    public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddHours(1);
}

public record GetEventsResult(IEnumerable<EventDto> EventDtos);

public record GetEventsResponseDto(IEnumerable<EventDto> EventDtos);

