using DeliveryApp.Core.Application.UseCases.DomainEventHandlers;
using DeliveryApp.Core.Domain.DomainEvents;
using DeliveryApp.Core.Ports;
using System.Linq;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.DomainEventHandlers
{
    public class OrderComplitedEventHandlerConfigurationTest
    {
        [Fact]
        public void OrderComplitedEventHandlerShouldBePublic()
        {
            Assert.True(typeof(OrderComplitedEventHandler).IsPublic);
        }

        [Fact]
        public void OrderComplitedEventHandlerShouldBeSeald()
        {
            Assert.True(typeof(OrderComplitedEventHandler).IsSealed);
        }

        [Fact]
        public void OrderComplitedEventHandlerShouldBeClass()
        {
            Assert.True(typeof(OrderComplitedEventHandler).IsClass);
        }

        [Fact]
        public void OrderComplitedEventHandlerShouldBeSubClassOfINotificationHandler()
        {
            Assert.Contains(typeof(INotificationHandler<OrderCompletedDomainEvent>), typeof(OrderComplitedEventHandler).GetInterfaces());
        }

        [Fact]
        public void OrderComplitedEventHandlerBaseConstructor_ShouldBeContains_IMessageBusProducer()
        {
            var type = typeof(OrderComplitedEventHandler);

            var constructors = type.GetConstructors();

            var constructor = constructors
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    return parameters.Length == 1 &&
                           parameters[0].ParameterType == typeof(IMessageBusProducer);
                });

            Assert.NotNull(constructor);
        }
    }
}