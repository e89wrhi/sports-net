using Duende.IdentityServer.EntityFramework.Entities;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sport.Common.Web;

namespace Match.Features.GetMatch.V1;

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
