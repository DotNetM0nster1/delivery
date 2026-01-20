using DeliveryApp.Core.Domain.DomainEvents;
using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.DomainEventHandlers
{
    public sealed class OrderCreatedEventHandler(IMessageBusProducer messageBusProducer) : INotificationHandler<OrderCreatedDomainEvent>
    {
        private readonly IMessageBusProducer _messageBusProducer = messageBusProducer;

        public async Task Handle(OrderCreatedDomainEvent orderCreatedDomainEvent, CancellationToken cancellationToken)
        {
            await _messageBusProducer.PublishOrderCreatedDomainEvent(orderCreatedDomainEvent, cancellationToken);
        }
    }
}