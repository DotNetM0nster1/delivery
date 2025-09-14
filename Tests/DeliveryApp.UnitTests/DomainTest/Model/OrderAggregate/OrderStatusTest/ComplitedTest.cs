using DeliveryApp.Core.Domain.Model.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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