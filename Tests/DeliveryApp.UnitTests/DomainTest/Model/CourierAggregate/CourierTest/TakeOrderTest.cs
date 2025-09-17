using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Microsoft.VisualBasic;
using Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.CourierTest
{
    public class TakeOrderTest
    {
        [Fact]
        public void WhenTakingOrder_AndOrderIsNull_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var x = 9;
            var y = 4;
            var currentCourierLocation = Location.Create(x, y);

            var courierSpeed = 5;
            var courierName = "Sevolod Seleznev";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation.Value).Value;

            Order order = null;

            //Act
            var takeOrderResult = courier.TakeOrder(order);

            //Assert
            Assert.True(takeOrderResult.IsFailure);
            Assert.False(takeOrderResult.IsSuccess);
            Assert.True(takeOrderResult.Error.InnerError.Code == "is.courier.can.take.order.error");
        }

        [Fact]
        public void WhenTakingOrder_AndCourierNoHaveMatchingStoragePlaceByVolume_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xCourier = 9;
            var yCourier = 4;
            var currentCourierLocation = Location.Create(xCourier, yCourier);

            var xOrder = 1;
            var yOrder = 2;
            var orderLocation = Location.Create(xOrder, yOrder);

            var orderVolume = 50;
            var backetId = Guid.NewGuid();
            var order = Order.Create(backetId, orderLocation.Value, orderVolume);

            var courierSpeed = 5;
            var courierName = "Sevolod Seleznev";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation.Value).Value;

            var orderId = Guid.NewGuid();
            var maxStoragePlaceVolume = 5;
            var storagePlaceName = "smallbag";

            courier.StoragePlaces.Clear();
            courier.AddNewStoragePlace(storagePlaceName, maxStoragePlaceVolume);

            //Act
            var takeOrderResult = courier.TakeOrder(order.Value);

            //Assert
            Assert.False(takeOrderResult.IsSuccess);
            Assert.True(takeOrderResult.IsFailure);
            Assert.True(takeOrderResult.Error.Code == "take.order.result.false.or.an.error.occured");
        }

        [Fact]
        public void WhenTakingOrder_AndCourierStoragePalceAlreadyHaveOrder_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xCourier = 3;
            var yCourier = 2;
            var currentCourierLocation = Location.Create(xCourier, yCourier).Value;

            var xOrder = 7;
            var yOrder = 5;
            var orderLocation = Location.Create(xOrder, yOrder);

            var orderVolume = 20;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation.Value, orderVolume).Value;

            var courierSpeed = 5;
            var courierName = "Saveliy Prostikin";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation).Value;

            var storagePlaceName = "bag";
            var maxStoragePlaceVolume = 20;

            courier.AddNewStoragePlace(storagePlaceName, maxStoragePlaceVolume);
            courier.TakeOrder(order);

            //Act
            var takeOrderResult = courier.TakeOrder(order);

            //Assert
            Assert.False(takeOrderResult.IsSuccess);
            Assert.True(takeOrderResult.IsFailure);
            Assert.True(takeOrderResult.Error.Code == "take.order.result.false.or.an.error.occured");
        }

        [Fact]
        public void WhenTakingOrder_AndCourierStoragePlaceVolumeIsCorrect_AndCourierStoragePlaceNoHaveOrder_ThenMethodShouldBeSetOrderInCourierStoragePlace()
        {
            //Arrange
            var xCourier = 6;
            var yCourier = 4;
            var currentCourierLocation = Location.Create(xCourier, yCourier).Value;

            var xOrder = 9;
            var yOrder = 9;
            var orderLocation = Location.Create(xOrder, yOrder).Value;

            var orderVolume = 10;
            var backetId = Guid.NewGuid();
            var order = Order.Create(backetId, orderLocation, orderVolume).Value;

            var courierSpeed = 5;
            var courierName = "Denis Yavotkin";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation).Value;

            //Act
            var takeOrderResult = courier.TakeOrder(order);

            //Assert
            Assert.True(takeOrderResult.IsSuccess);
            Assert.False(takeOrderResult.IsFailure);
            Assert.Contains(order.Id, courier.StoragePlaces.Select(storage => storage.OrderId));
        }
    }
}