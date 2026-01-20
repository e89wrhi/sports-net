using MediatR;
using Sport.Common.Core;

namespace Sport.Common.EventStoreDB;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{
}