using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Providers.OrderProvider.GetAllActive;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Providers.CourierProvider.GetAllBusy
{
    public class AllActiveOrdersModelProviderTest : ProviderBase<AllActiveOrdersModelProvider>
    {
        [Fact]
        public async Task WhenGettingAllActiveOrders_AndAllActiveCouriersCountIs0_WhenMethodShouldBeReturnGetAllNotComplitedOrdersRequestWithEmptyList()
        {
            //Arrange-Act
            var activeOrders = await ModelProvider.GetAllActiveAsync();

            //Assert
            activeOrders.Should().NotBeNull();
            activeOrders.Orders.Should().BeEmpty();
            activeOrders.Orders.Count.Should().Be(0);   
        }

        [Fact]
        public async Task WhenGettingAllActiveOrders_AndExistOnlyComplitedOrdersStatus_WhenMethodShouldBeReturnGetAllNotComplitedOrdersRequestWithEmptyList()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId,Location.Create(2, 4).Value, 5).Value;
            var courier = Courier.Create(5, "SomeName", Location.Create(2, 5).Value).Value;

            order.Assign(courier);
            order.Complete();

            await DatabaseContext.Orders.AddAsync(order);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var activeOrders = await ModelProvider.GetAllActiveAsync();

            //Assert
            activeOrders.Should().NotBeNull();
            activeOrders.Orders.Should().BeEmpty();
            activeOrders.Orders.Count.Should().Be(0);
        }

        [Fact]
        public async Task WhenGettingAllActiveOrders_AndExistOnlyOrderWithCreatedStatus_WhenMethodShouldBeReturnGetAllNotComplitedOrdersRequestWithOrderIdAndLocationInfo()
        {
            //Arrange
            var orderX = 2;
            var orderY = 3;
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, Location.Create(orderX, orderY).Value, 8).Value;

            await DatabaseContext.Orders.AddAsync(order);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var activeOrders = await ModelProvider.GetAllActiveAsync();

            //Assert
            activeOrders.Should().NotBeNull();
            activeOrders.Orders.Should().NotBeEmpty();
            activeOrders.Orders.Count.Should().Be(1);

            activeOrders.Orders.FirstOrDefault(order => 
                order.Id == orderId && 
                order.Location.X == orderX &&
                order.Location.Y == orderY).Should().NotBeNull();
        }

        [Fact]
        public async Task WhenGettingAllActiveOrders_AndExistOrderWithAssignedStatus_WhenMethodShouldBeReturnGetAllNotComplitedOrdersRequestWithOrderIdAndLocationInfo()
        {
            //Arrange
            var orderX = 4;
            var orderY = 6;
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, Location.Create(orderX, orderY).Value, 1).Value;
            var courier = Courier.Create(5, "SomeName", Location.Create(2, 5).Value).Value;

            order.Assign(courier);

            await DatabaseContext.Orders.AddAsync(order);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var activeOrders = await ModelProvider.GetAllActiveAsync();

            //Assert
            activeOrders.Should().NotBeNull();
            activeOrders.Orders.Should().NotBeEmpty();
            activeOrders.Orders.Count.Should().Be(1);

            activeOrders.Orders.FirstOrDefault(order =>
                order.Id == orderId &&
                order.Location.X == orderX &&
                order.Location.Y == orderY).Should().NotBeNull();
        }

        [Fact]
        public async Task WhenGettingAllActiveOrders_AndExistOrdersWithAssignedAndCreatedStatus_WhenMethodShouldBeReturnGetAllNotComplitedOrdersRequestWithOrderIdAndLocationInfo()
        {
            //Arrange
            var assignOrderX = 4;
            var assignOrderY = 6;
            var assignedOrderId = Guid.NewGuid();
            var orderWithAssignedStatus = Order.Create(assignedOrderId, Location.Create(assignOrderX, assignOrderY).Value, 1).Value;

            var createdOrderX = 3;
            var createdOrderY = 9;
            var createdOrderId = Guid.NewGuid();
            var orderWithCreatedStatus = Order.Create(createdOrderId, Location.Create(createdOrderX, createdOrderY).Value, 3).Value;

            var courier = Courier.Create(5, "SomeName", Location.Create(2, 5).Value).Value;

            orderWithAssignedStatus.Assign(courier);

            await DatabaseContext.Orders.AddRangeAsync([orderWithAssignedStatus, orderWithCreatedStatus]);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var activeOrders = await ModelProvider.GetAllActiveAsync();

            //Assert
            activeOrders.Should().NotBeNull();
            activeOrders.Orders.Should().NotBeEmpty();
            activeOrders.Orders.Count.Should().Be(2);

            activeOrders.Orders.FirstOrDefault(order =>
                order.Id == assignedOrderId &&
                order.Location.X == assignOrderX &&
                order.Location.Y == assignOrderY).Should().NotBeNull();

            activeOrders.Orders.FirstOrDefault(order =>
                order.Id == createdOrderId &&
                order.Location.X == createdOrderX &&
                order.Location.Y == createdOrderY).Should().NotBeNull();
        }
    }
}