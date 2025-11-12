using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Providers.CourierProvider.GetAllBusy;
using DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Providers;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Queries.OrderProvider.GetAllActive
{
    public class AllActiveOrdersResultTest : ProviderBase<AllBusyCouriersModelProvider>
    {
        [Fact]
        public async Task WhenGettingAllBusyCouriers_AndNotExistBusyCouriers_WhenMethodShouldBeReturnGetAllBusyCouriersRequestWithEmptyList()
        { 
            //Arrange 
            var allBusyCouriers = await ModelProvider.GetAllBusyCouriersAsync();

            //Act-Assert
            allBusyCouriers.Should().NotBeNull();
            allBusyCouriers.Couriers.Should().BeEmpty();
            allBusyCouriers.Couriers.Count.Should().Be(0);
        }

        [Fact]
        public async Task WhenGettingAllBusyCouriers_AndExistBusyCourier_ThenMethodShouldBeReturnGetAllBusyCouriersRequestWithIdAndNameAndLocationInfo()
        {
            //Arrange
            var courierX = 2;
            var courierY = 9;
            var courierName = "SomeName";
            var courier = Courier.Create(3, courierName, Location.Create(courierX, courierY).Value).Value;

            var orderid = Guid.NewGuid();
            var order = Order.Create(orderid, Location.Create(8, 4).Value, 5).Value;

            courier.TakeOrder(order);

            await DatabaseContext.Couriers.AddAsync(courier);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var allBusyCouriers = await ModelProvider.GetAllBusyCouriersAsync();

            //Assert
            allBusyCouriers.Should().NotBeNull();
            allBusyCouriers.Couriers.Should().NotBeNull();
            allBusyCouriers.Couriers.Should().NotBeEmpty();
            allBusyCouriers.Couriers.Count.Should().Be(1);

            allBusyCouriers.Couriers.FirstOrDefault(courier =>
                courier.Name == courierName &&
                courier.Location.X == courierX &&
                courier.Location.Y == courierY).Should().NotBeNull();
        }
    }
}
