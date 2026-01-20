using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using Primitives;
using MediatR;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres
{
    public sealed class UnitOfWork(ApplicationDatabaseContext applicationContext, IMediator mediator) : IUnitOfWork
    {
        private readonly ApplicationDatabaseContext _applicationContext = applicationContext;
        private readonly IMediator _mediator = mediator;

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _applicationContext.SaveChangesAsync(cancellationToken);
            await PublishDomainEventsAsync();
            return true;
        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEntities = _applicationContext
                .ChangeTracker
                .Entries<IAggregateRoot>()
                .Where(x => x.Entity.GetDomainEvents().Any());

            var domaintEvents = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents());

            foreach (var domainEvent in domaintEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            domainEntities
                .ToList()
                .ForEach(x => x.Entity.ClearDomainEvents());
        }
    }
}