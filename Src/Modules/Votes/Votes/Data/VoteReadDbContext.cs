using Sport.Common.Mongo;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Vote.Models;

namespace Vote.Data;

public class VoteReadDbContext : MongoDbContext
{
    public VoteReadDbContext(IOptions<MongoOptions> options) : base(options)
    {
        Vote = GetCollection<VoteReadModel>(nameof(Vote).Underscore());
    }

    public IMongoCollection<VoteReadModel> Vote { get; }
}