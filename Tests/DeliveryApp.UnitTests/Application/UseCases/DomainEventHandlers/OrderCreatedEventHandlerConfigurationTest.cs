using DeliveryApp.Core.Application.UseCases.DomainEventHandlers;
using DeliveryApp.Core.Domain.DomainEvents;
using DeliveryApp.Core.Ports;
using System.Linq;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.DomainEventHandlers
{
    public class OrderCreatedEventHandlerConfigurationTest
    {
        [Fact]
        public void OrderCreatedEventHandlerShouldBePublic()
        {
            Assert.True(typeof(OrderCreatedEventHandler).IsPublic);
        }

        [Fact]
        public void OrderCreatedEventHandlerShouldBeSeald()
        {
            Assert.True(typeof(OrderCreatedEventHandler).IsSealed);
        }

        [Fact]
        public void OrderCreatedDomainEventHandlerShouldBeClass()
        {
            Assert.True(typeof(OrderCreatedEventHandler).IsClass);
        }

        [Fact]
        public void OrderCreatedEventHandlerShouldBeSubClassOfINotificationHandler()
        {
            Assert.Contains(typeof(INotificationHandler<OrderCreatedDomainEvent>), typeof(OrderCreatedEventHandler).GetInterfaces());
        }

        [Fact]
        public void OrderComplitedEventHandlerBaseConstructor_ShouldBeContains_IMessageBusProducer()
        {
            var type = typeof(OrderCreatedEventHandler);

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