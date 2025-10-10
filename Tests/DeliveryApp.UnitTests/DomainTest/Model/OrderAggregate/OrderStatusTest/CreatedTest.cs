using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.OrderAggregate.OrderStatusTest
{
    public class CreatedTest
    {
        [Fact]
        public void CreatedStatus_ShouldBeCreated()
        {
            var createdStatus = OrderStatus.Created;

            Assert.Equal(createdStatus, OrderStatus.Created);
            Assert.Equal("created", createdStatus.Name);
        }
    }
}