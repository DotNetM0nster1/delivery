using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.OrderAggregate.OrderTest
{
    public class CompliteTest
    {
        [Fact]
        public void WhenComplitingOrder_AndOrderIsAssign_ThenMethodShouldBeCompliteOrder()
        {
            //Arrange
            var xOrder = 9;
            var yOrder = 7;
            var orderLocation = Location.Create(xOrder, yOrder).Value;

            var xCourier = 1;
            var yCourier = 1;
            var courierLocation = Location.Create(xCourier, yCourier).Value;

            var courierSpeed = 1;
            var courierName = "Some Name";
            var courier = Courier.Create(courierSpeed, courierName, courierLocation).Value;

            var orderVolume = 10;
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, orderLocation, orderVolume).Value;

            order.Assign(courier);

            //Act
            var compliteOrderResult = order.Complete();

            //Assert
            Assert.True(compliteOrderResult.IsSuccess);
            Assert.False(compliteOrderResult.IsFailure);
            Assert.True(order.Status == OrderStatus.Completed);
        }

        [Fact]
        public void WhenComplitingOrder_AndOrderIsNotAssign_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xOrder = 1;
            var yOrder = 2;
            var orderLocation = Location.Create(xOrder, yOrder).Value;

            var xCourier = 3;
            var yCourier = 4;
            var courierLocation = Location.Create(xCourier, yCourier).Value;

            var courierSpeed = 2;
            var courierName = "Some Name";
            var courier = Courier.Create(courierSpeed, courierName, courierLocation).Value;

            var orderVolume = 12;
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, orderLocation, orderVolume).Value;

            //Act
            var compliteOrderResult = order.Complete();

            //Assert
            Assert.False(compliteOrderResult.IsSuccess);
            Assert.True(compliteOrderResult.IsFailure);
            Assert.False(order.Status == OrderStatus.Completed);
            Assert.True(compliteOrderResult.Error.Code == "value.is.invalid");
        }
    }
}