using Sport.Common.Mongo;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Events.Events.Models;

namespace Events.Data;

public class EventReadDbContext : MongoDbContext
{
    public EventReadDbContext(IOptions<MongoOptions> options) : base(options)
    {
        Event = GetCollection<EventReadModel>(nameof(Event).Underscore());
    }

    public IMongoCollection<EventReadModel> Event { get; }
}