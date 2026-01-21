using Sport.Common.Core;
using Sport.Common.Web;

namespace Events.Events.Features.GetEvents.V1;


public record GetEvents(Guid Id) : IQuery<GetEventsResult>;

public record GetEventsResult(EventDto EventDto);

public record GetEventsResponseDto(EventDto EventDto);

public class GetEventsEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/event/{{id}}",
                async (Guid id, IMediator mediator, IMapper mapper, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetEvents(id), cancellationToken);

                    var response = result.Adapt<GetEventsResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization(nameof(ApiScope))
            .WithName("GetEvents")
            .WithApiVersionSet(builder.NewApiVersionSet("Event").Build())
            .Produces<GetEventsResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Event By Id")
            .WithDescription("Get Event By Id")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class GetEventsValidator : AbstractValidator<GetEvents>
{
    public GetEventsValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is required!");
    }
}

internal class GetEventsHandler : IQueryHandler<GetEvents, GetEventsResult>