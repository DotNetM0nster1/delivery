using DeliveryApp.Core.Domain.Model.CourierAggregate;
using CSharpFunctionalExtensions;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.StoragePlaceTest
{
    public class StoragePlaceConfigurationTests
    {
        [Fact]
        public void StoragePlaceShouldBePublic()
        {
            Assert.True(typeof(StoragePlace).IsPublic);
        }

        [Fact]
        public void StoragePlaceShouldBeSealed()
        {
            Assert.True(typeof(StoragePlace).IsSealed);
        }

        [Fact]
        public void StoragePlaceShouldBeClass()
        {
            Assert.True(typeof(StoragePlace).IsClass);
        }

        [Fact]
        public void StoragePlaceShouldBeSubClassOfEntityGuid()
        {
            Assert.True(typeof(StoragePlace).IsSubclassOf(typeof(Entity<Guid>)));
        }
    }
}