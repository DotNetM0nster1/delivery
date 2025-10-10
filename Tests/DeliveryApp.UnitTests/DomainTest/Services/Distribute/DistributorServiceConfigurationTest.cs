using DeliveryApp.Core.Domain.Services.Distribute;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Services.Distribute
{
    public class DistributorServiceConfigurationTest
    {
        [Fact]
        public void CourierDistributorServiceShouldBePublic()
        {
            Assert.True(typeof(CourierDistributorService).IsPublic);
        }

        [Fact]
        public void CourierDistributorServiceShouldBeSealed()
        {
            Assert.True(typeof(CourierDistributorService).IsSealed);
        }

        [Fact]
        public void CourierDistributorServiceBeClass()
        {
            Assert.True(typeof(CourierDistributorService).IsClass);
        }

        [Fact]
        public void CourierDistributorServiceShouldBeImplementICourierDistributorService()
        {
            Assert.Contains(typeof(ICourierDistributorService), typeof(CourierDistributorService).GetInterfaces());
        }
    }
}