using DeliveryApp.Core.Domain.DomainEvents;

namespace DeliveryApp.Core.Ports
{
    public interface IMessageBusProducer
    {
        Task PublishOrderCompletedDomainEvent(OrderCompletedDomainEvent orderCompletedDomainEvent, CancellationToken cancellationToken);

        Task PublishOrderCreatedDomainEvent(OrderCreatedDomainEvent orderCreatedDomainEvent, CancellationToken cancellationToken);
    }
}