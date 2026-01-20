using Sport.Common.Core;

namespace Sport.Common.Mango;

/// <summary>
/// A specialized repository interface for MongoDB.
/// It bridges our generic repository pattern with the Mongo-specific implementation.
/// </summary>
public interface IMongoRepository<TEntity, in TId> : IRepository<TEntity, TId>
    where TEntity : class, IAggregate<TId>
{
}