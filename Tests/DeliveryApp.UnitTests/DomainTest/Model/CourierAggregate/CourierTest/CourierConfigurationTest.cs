using DeliveryApp.Core.Domain.Model.CourierAggregate;
using CSharpFunctionalExtensions;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.CourierTest
{
    public class CourierConfigurationTest
    {
        [Fact]
        public void CourierShouldBePublic()
        {
            Assert.True(typeof(Courier).IsPublic);
        }

        [Fact]
        public void CourierShouldBeSealed()
        {
            Assert.True(typeof(Courier).IsSealed);
        }

        [Fact]
        public void CourierShouldBeClass()
        {
            Assert.True(typeof(Courier).IsClass);
        }

        [Fact]
        public void CourierShouldBeSubClassOfEntityGuid()
        {
            Assert.True(typeof(Courier).IsSubclassOf(typeof(Entity<Guid>)));
        }
    }
}