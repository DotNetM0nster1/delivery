using DeliveryApp.Core.Application.UseCases.DomainEventHandlers;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Core.Domain.DomainEvents;
using DeliveryApp.Core.Ports;
using System.Threading.Tasks;
using System.Threading;
using NSubstitute;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.DomainEventHandlers
{
    public class OrderCreatedEventHandlerTest
    {
        private readonly IMessageBusProducer _messageBusProducerMoq = Substitute.For<IMessageBusProducer>();
        private readonly OrderCreatedEventHandler _orderCreatedEventHanbdler;

        public OrderCreatedEventHandlerTest()
        {
            _orderCreatedEventHanbdler = new OrderCreatedEventHandler(_messageBusProducerMoq);
        }

        [Fact]
        public async Task WhenHandlingOrderCreatedEvent_AndInputDataCorrect_WhenMethodShouldBeCallOrderCreatingEvent()
        {
            //Arrange
            var order = Order.Create(Guid.NewGuid(), Location.Create(3, 5).Value, 3).Value;

            var domainEvent = new OrderCreatedDomainEvent(order);

            //Act
            await _orderCreatedEventHanbdler.Handle(domainEvent, CancellationToken.None);

            //Assert
            await _messageBusProducerMoq.Received(1).PublishOrderCreatedDomainEvent(domainEvent, CancellationToken.None);
        }
    }
}