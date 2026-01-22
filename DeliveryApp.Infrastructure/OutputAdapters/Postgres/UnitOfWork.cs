using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Entities;
using Newtonsoft.Json;
using Primitives;
using MediatR;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres
{
    public sealed class UnitOfWork(ApplicationDatabaseContext applicationContext, IMediator mediator) : IUnitOfWork
    {
        private readonly ApplicationDatabaseContext _databaseContext = applicationContext;
        private readonly IMediator _mediator = mediator;

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await SaveDomainEventsInOutboxAsync();
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task SaveDomainEventsInOutboxAsync()
        {
            var outboxMessage = _databaseContext
                .ChangeTracker
                .Entries<IAggregateRoot>()
                .Select(x => x.Entity)
                    .SelectMany(aggreagate =>
                    {
                        var domainEvents = aggreagate.GetDomainEvents();
                        aggreagate.ClearDomainEvents();
                        return domainEvents;
                    })
                    .Select(domainEvent => new OutboxMessage
                    {
                        Id = domainEvent.EventId,
                        OccuredAtUtc = domainEvent.OccurredAt,
                        Type = domainEvent.GetType().Name,
                        Content = JsonConvert.SerializeObject(
                            domainEvent,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All,
                            })
                    })
                .ToList();

            await _databaseContext.Set<OutboxMessage>().AddRangeAsync(outboxMessage);
        }
    }
}