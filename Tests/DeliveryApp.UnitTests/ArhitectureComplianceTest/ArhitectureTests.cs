using DeliveryApp.Core.Application.UseCases.Queries.Order.GetAllNotComplitedOrders;
using DeliveryApp.Core.Application.UseCases.Queries.Courier.GetAllBusyCouriers;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveCouriers;
using DeliveryApp.Core.Application.UseCases.Commands.Order.AssignOrder;
using DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using NetArchTest.Rules;
using Xunit;

namespace DeliveryApp.UnitTests.ArhitectureComplianceTest
{
    public class ArhitectureTests
    {
        private const string DeliveryInfrastructure = "DeliveryApp.Infrastructure";

        [Fact]
        public void CourierAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(Courier).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void StoragePlaceAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(StoragePlace).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void OrderAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(Order).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void OrderStatusAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(OrderStatus).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void LocationAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(Location).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void MoveCouriersCommandAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(MoveCouriersCommand).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void MoveCouriersHandlerAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(Core.Application.UseCases.Commands.Courier.MoveCouriers.MoveCouriersHandler).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void AssignOrderCommandAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(AssignOrderCommand).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void AssignOrderHandlerAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(AssignOrderHandler).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void CreateOrderCommandAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(CreateOrderCommand).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void CreateOrderHandlerAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(CreateOrderHandler).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void GetAllBusyCouriersHandlerAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(GetAllBusyCouriersHandler).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void GetAllBusyCouriersRequestAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(GetAllBusyCouriersRequest).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void CourierDtoAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(CourierDto).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void LocationDtoAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(LocationDto).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void OrderDtoAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(OrderDto).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void GetAllNotComplitedOrdersHandlerAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(GetAllNotComplitedOrdersHandler).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }

        [Fact]
        public void GetAllNotComplitedOrdersRequestAssembly_ShouldntBeReference_OnInfrastructure()
        {
            var result = Types
                .InAssembly(typeof(GetAllNotComplitedOrdersRequest).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DeliveryInfrastructure)
                .GetResult();

            Assert.True(result.IsSuccessful, "Слой Core не должен зависеть от слоя Infrastructure!");
        }
    }
}