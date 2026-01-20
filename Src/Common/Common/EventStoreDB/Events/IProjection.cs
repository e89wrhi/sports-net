namespace Sport.Common.EventStoreDB;

public interface IProjection
{
    void When(object @event);
}