using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.OrderAggregate.OrderStatusTest
{
    public class OrderStatusConfigurationTest
    {
        [Fact]
        public void OrderStatusShouldBePublic()
        {
            Assert.True(typeof(OrderStatus).IsPublic);
        }

        [Fact]
        public void OrderStatusShouldBeSealed()
        {
            Assert.True(typeof(OrderStatus).IsSealed);
        }

        [Fact]
        public void OrderStatusShouldBeClass()
        {
            Assert.True(typeof(OrderStatus).IsClass);
        }

        [Fact]
        public void OrderStatusShouldBeSubClassOfValueObject()
        {
            Assert.True(typeof(OrderStatus).IsSubclassOf(typeof(ValueObject)));
        }
    }
}