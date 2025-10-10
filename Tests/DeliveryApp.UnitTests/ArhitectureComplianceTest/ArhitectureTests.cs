using NetArchTest.Rules;
using Xunit;

namespace DeliveryApp.UnitTests.ArhitectureComplianceTest
{
    public class ArhitectureTests
    {
        private const string DeliveryCore = "DeliveryApp.Core";
        private const string DeliveryApiCore = "DeliveryApp.Api";
        private const string DeliveryInfrastructure = "DeliveryApp.Infrastructure";

        [Fact]
        public void CourierAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(DeliveryApp.Core.Domain.Model.CourierAggregate.Courier).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void StoragePlaceAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(DeliveryApp.Core.Domain.Model.CourierAggregate.StoragePlace).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void OrderAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(DeliveryApp.Core.Domain.Model.OrderAggregate.Order).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void OrderStatusAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(DeliveryApp.Core.Domain.Model.OrderAggregate.OrderStatus).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void LocationAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(DeliveryApp.Core.Domain.Model.SharedKernel.Location).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }
    }
}