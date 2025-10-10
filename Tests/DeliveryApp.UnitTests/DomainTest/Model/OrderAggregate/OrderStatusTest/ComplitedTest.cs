using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.OrderAggregate.OrderStatusTest
{
    public class ComplitedTest
    {
        [Fact]
        public void ComplitedStatus_ShouldBeCreated()
        {
            var complitedStatus = OrderStatus.Completed;

            Assert.Equal(complitedStatus, OrderStatus.Completed);
            Assert.Equal("completed", complitedStatus.Name);
        }
    }
}