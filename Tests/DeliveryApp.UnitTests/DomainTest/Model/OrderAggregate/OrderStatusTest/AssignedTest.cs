using DeliveryApp.Core.Domain.Model.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.OrderAggregate.OrderStatusTest
{
    public class AssignedTest
    {
        [Fact]
        public void AssignedStatus_ShouldBeCreated()
        {
            var assignedStatus = OrderStatus.Assigned;

            Assert.Equal(assignedStatus, OrderStatus.Assigned);
            Assert.Equal("assigned", assignedStatus.Name);
        }
    }
}