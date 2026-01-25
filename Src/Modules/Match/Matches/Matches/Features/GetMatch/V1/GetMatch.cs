using Ardalis.GuardClauses;
using Duende.IdentityServer.EntityFramework.Entities;
using Match.Data;
using Match.Matches.Dtos;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Caching;
using Sport.Common.Core;
using Sport.Common.Web;
using Sport.Matchs.Exceptions;
using FluentValidation;

namespace Match.Matches.Features.GetMatch.V1;

public record GetMatchById(Guid Id) : IQuery<GetMatchByIdResult>;

public record GetMatchByIdResult(MatchDto MatchDto);

public record GetMatchByIdResponseDto(MatchDto MatchDto);

public class GetMatchByIdEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/match/{{id}}",
                async (Guid id, IMediator mediator, IMapper mapper, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetMatchById(id), cancellationToken);

                    var response = result.Adapt<GetMatchByIdResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("GetMatchById")
            .WithApiVersionSet(builder.NewApiVersionSet("Match").Build())
            .Produces<GetMatchByIdResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Match By Id")
            .WithDescription("Get Match By Id")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

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
    private readonly MatchReadDbContext _matchReadDbContext;

    public GetMatchByIdHandler(IMapper mapper, MatchReadDbContext matchReadDbContext)
    {
        _mapper = mapper;
        _matchReadDbContext = matchReadDbContext;
    }

    public async Task<GetMatchByIdResult> Handle(GetMatchById request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var match = await _matchReadDbContext.Match.AsQueryable().SingleOrDefaultAsync(
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