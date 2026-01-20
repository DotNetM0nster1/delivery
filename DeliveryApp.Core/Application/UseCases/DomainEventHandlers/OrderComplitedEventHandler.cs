using DeliveryApp.Core.Domain.DomainEvents;
using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.DomainEventHandlers
{
    public sealed class OrderComplitedEventHandler(IMessageBusProducer messageBusProducer) : INotificationHandler<OrderCompletedDomainEvent>
    {
        private readonly IMessageBusProducer _messageBusProducer = messageBusProducer;

        public async Task Handle(OrderCompletedDomainEvent orderCompletedDomainEvent, CancellationToken cancellationToken)
        {
            await _messageBusProducer.PublishOrderCompletedDomainEvent(orderCompletedDomainEvent, cancellationToken);
        }
    }
}