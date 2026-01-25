using Ardalis.GuardClauses;
using Event.Data;
using Event.Events.Features.DeleteEvent.V1;
using Event.Events.Models;
using MapsterMapper;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Events.Exceptions;

namespace Event.Events.Features.DeleteEvent.V1;

public record DeleteEventMongo(Guid Id): InternalCommand;

public class DeleteEventMongoHandler : ICommandHandler<DeleteEventMongo>
{
    private readonly EventReadDbContext _readDbContext;
    private readonly IMapper _mapper;

    public DeleteEventMongoHandler(
        EventReadDbContext readDbContext,
        IMapper mapper)
    {
        _readDbContext = readDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteEventMongo request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var eventReadModel = _mapper.Map<EventReadModel>(request);

        var @event = await _readDbContext.Event.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == eventReadModel.Id, cancellationToken);

        if (@event is null)
        {
            throw new EventNotFoundException(request.Id);
        }

        await _readDbContext.Event.DeleteOneAsync(x => x.Id == eventReadModel.Id, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
