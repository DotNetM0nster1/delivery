using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;
using DeliveryApp.Core.Ports;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Repositories
{
    public class OrderRepositoryConfigurationTest
    {
        [Fact]
        public void OrderRepositoryShouldBePublic()
        {
            Assert.True(typeof(OrderRepository).IsPublic);
        }

        [Fact]
        public void OrderRepositoryShouldBeSealed()
        {
            Assert.True(typeof(OrderRepository).IsSealed);
        }

        [Fact]
        public void OrderRepositoryShouldBeClass()
        {
            Assert.True(typeof(OrderRepository).IsClass);
        }

        [Fact]
        public void OrderRepositoryShouldBeSubClassOfValueObject()
        {
            Assert.Contains(typeof(IOrderRepository), typeof(OrderRepository).GetInterfaces());
        }
    }
}