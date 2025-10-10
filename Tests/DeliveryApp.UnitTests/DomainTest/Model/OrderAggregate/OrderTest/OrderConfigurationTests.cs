using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Primitives;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.OrderAggregate.OrderTest
{
    public class OrderConfigurationTests
    {
        [Fact]
        public void OrderShouldBePublic()
        {
            Assert.True(typeof(Order).IsPublic);
        }

        [Fact]
        public void OrderShouldBeSealed()
        {
            Assert.True(typeof(Order).IsSealed);
        }

        [Fact]
        public void OrderShouldBeClass()
        {
            Assert.True(typeof(Order).IsClass);
        }

        [Fact]
        public void OrderShouldBeSubClassOfAggreagateGuid()
        {
            Assert.True(typeof(Order).IsSubclassOf(typeof(Aggregate<Guid>)));
        }
    }
}