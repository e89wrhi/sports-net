using Sport.Common.Core;
using Sport.Common.PersistMessageProcessor;
using Sport.Common.Polly;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Transactions;

namespace Vote.Data;

public class EfTxVoteBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : notnull, IRequest<TResponse>
where TResponse : notnull
{
    private readonly ILogger<EfTxVoteBehavior<TRequest, TResponse>> _logger;
    private readonly VoteDbContext _dbContext;
    private readonly IPersistMessageDbContext _persistMessageDbContext;
    private readonly IEventDispatcher _eventDispatcher;

    public EfTxVoteBehavior(
        ILogger<EfTxVoteBehavior<TRequest, TResponse>> logger,
        VoteDbContext dbContext,
        IPersistMessageDbContext persistMessageDbContext,
        IEventDispatcher eventDispatcher
    )
    {
        _logger = logger;
        _dbContext = dbContext;
        _persistMessageDbContext = persistMessageDbContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "{Prefix} Handled command {MediatrRequest}",
            GetType().Name,
            typeof(TRequest).FullName);

        _logger.LogDebug(
            "{Prefix} Handled command {MediatrRequest} with content {RequestContent}",
            GetType().Name,
            typeof(TRequest).FullName,
            JsonSerializer.Serialize(request));

        var response = await next();

        _logger.LogInformation(
            "{Prefix} Executed the {MediatrRequest} request",
            GetType().Name,
            typeof(TRequest).FullName);

        while (true)
        {
            var domainEvents = _dbContext.GetDomainEvents();

            if (domainEvents is null || !domainEvents.Any())
            {
                return response;
            }

            _logger.LogInformation(
                "{Prefix} Open the transaction for {MediatrRequest}",
                GetType().Name,
                typeof(TRequest).FullName);

            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);

            await _eventDispatcher.SendAsync(
                domainEvents.ToArray(),
                typeof(TRequest),
                cancellationToken);

            // Save data to database with some retry policy in distributed transaction
            await _dbContext.RetryOnFailure(
                async () =>
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                });

            // Save data to database with some retry policy in distributed transaction
            await _persistMessageDbContext.RetryOnFailure(
                async () =>
                {
                    await _persistMessageDbContext.SaveChangesAsync(cancellationToken);
                });

            scope.Complete();

            return response;
        }
    }
}