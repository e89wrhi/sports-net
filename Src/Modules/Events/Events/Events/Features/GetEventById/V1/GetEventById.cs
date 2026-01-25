using Ardalis.GuardClauses;
using Event.Data;
using Event.Events.Dtos;
using Mapster;
using MapsterMapper;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Events.Exceptions;
using FluentValidation;

namespace Event.Events.Features.GetEventById.V1;

public record GetEventById(Guid Id) : IQuery<GetEventByIdResult>;

public record GetEventByIdResult(EventDto EventDto);

public class GetEventByIdValidator : AbstractValidator<GetEventById>
{
    public GetEventByIdValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is required!");
    }
}

internal class GetEventByIdHandler : IQueryHandler<GetEventById, GetEventByIdResult>
{
    private readonly IMapper _mapper;
    private readonly EventReadDbContext _eventReadDbContext;

    public GetEventByIdHandler(IMapper mapper, EventReadDbContext eventReadDbContext)
    {
        _mapper = mapper;
        _eventReadDbContext = eventReadDbContext;
    }

    public async Task<GetEventByIdResult> Handle(GetEventById request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var @event = await _eventReadDbContext.Event.AsQueryable().SingleOrDefaultAsync(
            x => x.Id == request.Id, cancellationToken);

        if (@event is null)
        {
            throw new EventNotFoundException(request.Id);
        }

        var eventDto = _mapper.Map<EventDto>(@event);

        return new GetEventByIdResult(eventDto);
    }
}
