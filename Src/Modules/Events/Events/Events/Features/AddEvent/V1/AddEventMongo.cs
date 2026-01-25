namespace Event.Events.Features.AddEvent.V1;

using Ardalis.GuardClauses;
using Event.Data;
using Event.Events.Models;
using MapsterMapper;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sport.Common.Core;
using Sport.Events.Exceptions;
using System;

public record AddEventMongo(
    Guid Id,
    Guid MatchId,
    string Title,
    DateTime Time,
    string Type): InternalCommand;

public class AddEventMongoHandler : ICommandHandler<AddEventMongo>
{
    private readonly EventReadDbContext _readDbContext;
    private readonly IMapper _mapper;

    public AddEventMongoHandler(
        EventReadDbContext readDbContext,
        IMapper mapper)
    {
        _readDbContext = readDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddEventMongo request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var eventReadModel = _mapper.Map<EventReadModel>(request);

        var @event = await _readDbContext.Event.AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == eventReadModel.Id, cancellationToken);

        if (@event is not null)
        {
            throw new EventAlreadyExistException(request.Id);
        }

        await _readDbContext.Event.InsertOneAsync(eventReadModel, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
