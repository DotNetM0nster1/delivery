using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;
using DeliveryApp.Core.Ports;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Repositories
{
    public class CourierRepositoryConfigurationTests
    {
        [Fact]
        public void CourierRepositoryShouldBePublic()
        {
            Assert.True(typeof(CourierRepository).IsPublic);
        }

        [Fact]
        public void CourierRepositoryShouldBeSealed()
        {
            Assert.True(typeof(CourierRepository).IsSealed);
        }

        [Fact]
        public void CourierRepositoryShouldBeClass()
        {
            Assert.True(typeof(CourierRepository).IsClass);
        }

        [Fact]
        public void CourierRepositoryShouldBeSubClassOfValueObject()
        {
            Assert.Contains(typeof(ICourierRepository), typeof(CourierRepository).GetInterfaces());
        }
    }
}