namespace Events.Events.Features.AddEvent.V1;

using Sport.Common.Core;
using System;

public record AddEventMongo()
{
}

public class AddEventMongoHandler : ICommandHandler<CreateEventMongo>
{
    private readonly EventReadDbContext _eventReadDbContext;
    private readonly IMapper _mapper;

    public CreateEventMongoHandler(
        EventReadDbContext eventReadDbContext,
        IMapper mapper)
    {
        _eventReadDbContext = eventReadDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateEventMongo request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var eventReadModel = _mapper.Map<EventReadModel>(request);

        var event = await _eventReadDbContext.Event.AsQueryable()
            .FirstOrDefaultAsync(x => x.EventId == eventReadModel.EventId && !x.IsDeleted, cancellationToken);

        if (event is not null)
        {
            throw new EventAlreadyExistException();
        }

        await _eventReadDbContext.Event.InsertOneAsync(eventReadModel, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}