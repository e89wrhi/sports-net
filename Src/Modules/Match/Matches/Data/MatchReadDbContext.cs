using Sport.Common.Mongo;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Matches.Matches.Models;

namespace Matches.Data;

public class MatchReadDbContext : MongoDbContext
{
    public MatchReadDbContext(IOptions<MongoOptions> options) : base(options)
    {
        Match = GetCollection<MatchReadModel>(nameof(Match).Underscore());
    }

    public IMongoCollection<MatchReadModel> Match { get; }
}