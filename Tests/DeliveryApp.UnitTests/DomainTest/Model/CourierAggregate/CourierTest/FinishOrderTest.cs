using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.CourierTest
{
    public class FinishOrderTest
    {
        [Fact]
        public void WhenFinishingOrder_AndOrderIdIsEmpty_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xCourier = 8;
            var yCourier = 3;
            var currentCourierLocation = Location.Create(xCourier, yCourier).Value;

            var courierSpeed = 2;
            var courierName = "Mihail Rudakov";
            var courier = Courier.Create(courierSpeed,courierName, currentCourierLocation).Value;

            //Act
            var finishOrderResult = courier.FinishOrder(Guid.Empty);

            //Assert
            Assert.True(finishOrderResult.IsFailure);
            Assert.False(finishOrderResult.IsSuccess);
            Assert.True(finishOrderResult.Error.Code == "value.is.required");
        }

        [Fact]
        public void WhenFinishingOrder_AndCourierNoHaveStoragePlaces_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xCourier = 8;
            var yCourier = 3;
            var currentCourierLocation = Location.Create(xCourier, yCourier).Value;

            var xOrder = 5;
            var yOrder = 6;
            var orderLocation = Location.Create(xOrder, yOrder).Value;

            var courierSpeed = 2;
            var courierName = "Alexey Sushilov";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation).Value;

            var orderVolume = 10;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            courier.TakeOrder(order);
            courier.CourierStoragePlaces.Clear();

            //Act
            var finishOrderResult = courier.FinishOrder(order.Id);

            //Assert
            Assert.True(finishOrderResult.IsFailure);
            Assert.False(finishOrderResult.IsSuccess);
            Assert.True(finishOrderResult.Error.Code == "value.is.required");
        }

        [Fact]
        public void WhenFinishingOrder_AndOrderNotExistOnSomeOfStoragePlace_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xCourier = 8;
            var yCourier = 3;
            var currentCourierLocation = Location.Create(xCourier, yCourier).Value;

            var xOrder = 5;
            var yOrder = 6;
            var orderLocation = Location.Create(xOrder, yOrder).Value;

            var courierSpeed = 2;
            var courierName = "Mihail Vardonov";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation).Value;

            var orderVolume = 10;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var finishOrderResult = courier.FinishOrder(order.Id);

            //Assert
            Assert.True(finishOrderResult.IsFailure);
            Assert.False(finishOrderResult.IsSuccess);
            Assert.True(finishOrderResult.Error.Code == "value.is.required");
        }

        [Fact]
        public void WhenFinishingOrder_AndOrderExistInStorage_ThenMethodShouldBeSetNullOnOrderIdInStoragePlace()
        {
            //Arrange
            var xCourier = 5;
            var yCourier = 6;
            var currentCourierLocation = Location.Create(xCourier, yCourier).Value;

            var xOrder = 3;
            var yOrder = 9;
            var orderLocation = Location.Create(xOrder, yOrder).Value;

            var courierSpeed = 2;
            var courierName = "Kirill Varonov";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation).Value;

            var orderVolume = 10;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;
            courier.TakeOrder(order);

            //Act
            var finishOrderResult = courier.FinishOrder(order.Id);

            //Assert
            Assert.True(finishOrderResult.IsSuccess);
            Assert.False(finishOrderResult.IsFailure);
            Assert.True(courier.CourierStoragePlaces[0].OrderId == null);
        }
    }
}