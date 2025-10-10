using DeliveryApp.Core.Domain.Model.SharedKernel;
using CSharpFunctionalExtensions;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.SharedKernel.LocationTests
{
    public class LocationConfigurationTests
    {
        [Fact]
        public void StoragePlaceShouldBePublic()
        {
            Assert.True(typeof(Location).IsPublic);
        }

        [Fact]
        public void StoragePlaceShouldBeSealed()
        {
            Assert.True(typeof(Location).IsSealed);
        }

        [Fact]
        public void StoragePlaceShouldBeClass()
        {
            Assert.True(typeof(Location).IsClass);
        }

        [Fact]
        public void StoragePlaceShouldBeSubClassOfEntityGuid()
        {
            Assert.True(typeof(Location).IsSubclassOf(typeof(ValueObject)));
        }
    }
}