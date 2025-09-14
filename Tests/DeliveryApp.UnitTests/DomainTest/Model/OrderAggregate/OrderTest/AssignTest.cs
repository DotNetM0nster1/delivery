using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.OrderAggregate.OrderTest
{
    public class AssignTest
    {
        [Fact]
        public void WhenAssignOrder_AndCourierNotNull_AndOrderStatusIsCreated_ThenMethodShouldBeSetAssignStatusAndCourierIdToOrder()
        {
            //Arrange
            var xOrder = 9;
            var yOrder = 7;
            var orderLocation = Location.Create(xOrder, yOrder).Value;

            var xCourier = 1;
            var yCourier = 1;
            var courierLocation = Location.Create(xCourier, yCourier).Value;

            var courierSpeed = 6;
            var courierName = "Artem Orehov";
            var courier = Courier.Create(courierSpeed, courierName, courierLocation).Value;

            var orderVolume = 10;
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, orderLocation, orderVolume).Value;

            //Act
            var assignOrderResult = order.Assign(courier);

            //Assert
            Assert.False(assignOrderResult.IsFailure);
            Assert.True(assignOrderResult.IsSuccess);
            Assert.Equal(courier.Id, order.CourierId);
            Assert.Equal(order.Status, OrderStatus.Assigned);
        }

        [Fact]
        public void WhenAssignOrder_AndCourierNull_AndOrderStatusIsCreated_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xOrder = 5;
            var yOrder = 6;
            var orderLocation = Location.Create(xOrder, yOrder).Value;

            var xCourier = 9;
            var yCourier = 9;
            var courierLocation = Location.Create(xCourier, yCourier).Value;

            var orderVolume = 20;
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, orderLocation, orderVolume).Value;

            Courier courier = null;

            //Act
            var assignOrderResult = order.Assign(courier);

            //Assert
            Assert.True(assignOrderResult.IsFailure);
            Assert.False(assignOrderResult.IsSuccess);
            Assert.True(assignOrderResult.Error.Code == "value.is.required");
        }
    }
}