using Match.Dtos;
using Sport.Common.Core;

namespace Match.Features.GetMatch.V1;

public record GetMatchById(Guid Id) : IQuery<GetMatchByIdResult>;

public record GetMatchByIdResult(MatchDto MatchDto);

public record GetMatchByIdResponseDto(MatchDto MatchDto);

