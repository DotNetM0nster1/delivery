using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Primitives;
using MediatR;
using Quartz;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.BackgroundJobs
{
    public sealed class ProccessOutboxMessagesJob(ApplicationDatabaseContext databaseContext, IMediator mediator) : IJob
    {
        private readonly ApplicationDatabaseContext _databaseContext = databaseContext;
        private readonly IMediator _mediator = mediator;

        public async Task Execute(IJobExecutionContext context)
        {
            var messageButchCount = 20;

            var outboxMessages = await _databaseContext
                .Set<OutboxMessage>()
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccuredAtUtc)
                .Take(messageButchCount)
                .ToListAsync(context.CancellationToken);

            if (outboxMessages.Any())
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                foreach (var outboxMessage in outboxMessages)
                {
                    var domainEvent = JsonConvert.DeserializeObject<DomainEvent>(outboxMessage.Content, settings);

                    await _mediator.Publish(domainEvent);

                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                }

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}