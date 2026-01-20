using DeliveryApp.Core.Application.UseCases.DomainEventHandlers;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Core.Domain.DomainEvents;
using System.Threading.Tasks;
using DeliveryApp.Core.Ports;
using System.Threading;
using NSubstitute;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.DomainEventHandlers
{
    public class OrderComplitedEventHandlerTest
    {
        private readonly IMessageBusProducer _messageBusProducerMoq = Substitute.For<IMessageBusProducer>();
        private readonly OrderComplitedEventHandler _orderComplitedEventHanbdler;

        public OrderComplitedEventHandlerTest() 
        {
            _orderComplitedEventHanbdler = new OrderComplitedEventHandler(_messageBusProducerMoq);
        }

        [Fact]
        public async Task WhenHandlingOrderComplitedEvent_AndInputDataCorrect_WhenMethodShouldBeCallOrderComplitingEvent()
        {
            //Arrange
            var courier = Courier.Create(3, "Ivan Maslov", Location.Create(5,6).Value).Value;
            var order = Order.Create(Guid.NewGuid(), Location.Create(3,5).Value, 3).Value;

            order.Assign(courier);

            var domainEvent = new OrderCompletedDomainEvent(order);

            //Act
            await _orderComplitedEventHanbdler.Handle(domainEvent, CancellationToken.None);

            //Assert
            await _messageBusProducerMoq.Received(1).PublishOrderCompletedDomainEvent(domainEvent, CancellationToken.None);
        }
    }
}