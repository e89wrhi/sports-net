using Ardalis.GuardClauses;
using Vote.Data;
using Vote.Votes.Dtos;
using Mapster;
using MapsterMapper;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Votes.Exceptions;
using FluentValidation;

namespace Vote.Votes.Features.GetVoteById.V1;

public record GetVoteById(Guid Id) : IQuery<GetVoteByIdResult>;

public record GetVoteByIdResult(VoteDto VoteDto);

public class GetVoteByIdValidator : AbstractValidator<GetVoteById>
{
    public GetVoteByIdValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is required!");
    }
}

internal class GetVoteByIdHandler : IQueryHandler<GetVoteById, GetVoteByIdResult>
{
    private readonly IMapper _mapper;
    private readonly VoteReadDbContext _voteReadDbContext;

    public GetVoteByIdHandler(IMapper mapper, VoteReadDbContext voteReadDbContext)
    {
        _mapper = mapper;
        _voteReadDbContext = voteReadDbContext;
    }

    public async Task<GetVoteByIdResult> Handle(GetVoteById request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var vote = await _voteReadDbContext.Vote.AsQueryable().SingleOrDefaultAsync(
            x => x.Id == request.Id, cancellationToken);

        if (vote is null)
        {
            throw new VoteNotFoundException(request.Id);
        }

        var voteDto = _mapper.Map<VoteDto>(vote);

        return new GetVoteByIdResult(voteDto);
    }
}
