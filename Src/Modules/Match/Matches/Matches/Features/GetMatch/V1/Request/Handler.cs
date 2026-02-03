using Ardalis.GuardClauses;
using FluentValidation;
using MapsterMapper;
using Match.Data;
using Match.Dtos;
using Microsoft.EntityFrameworkCore;
using Sport.Common.Core;
using Sport.Matchs.Exceptions;

namespace Match.Features.GetMatch.V1;

public class GetMatchByIdValidator : AbstractValidator<GetMatchById>
{
    public GetMatchByIdValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is required!");
    }
}

internal class GetMatchByIdHandler : IQueryHandler<GetMatchById, GetMatchByIdResult>
{
    private readonly IMapper _mapper;
    private readonly MatchDbContext _matchDbContext;

    public GetMatchByIdHandler(IMapper mapper, MatchDbContext matchDbContext)
    {
        _mapper = mapper;
        _matchDbContext = matchDbContext;
    }

    public async Task<GetMatchByIdResult> Handle(GetMatchById request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var match = await _matchDbContext.Matches.AsQueryable().SingleOrDefaultAsync(
            x => x.Id == request.Id &&
                             !x.IsDeleted, cancellationToken);

        if (match is null)
        {
            throw new MatchNotFoundException(request.Id);
        }

        var matchDto = _mapper.Map<MatchDto>(match);

        return new GetMatchByIdResult(matchDto);
    }
}

