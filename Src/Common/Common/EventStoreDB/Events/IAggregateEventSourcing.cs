using Sport.Common.Core;

namespace Sport.Common.EventStoreDB;

public interface IAggregateEventSourcing : IProjection, IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}

public interface IAggregateEventSourcing<T> : IAggregateEventSourcing, IEntity<T>
{
}