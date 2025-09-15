using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Microsoft.VisualBasic;
using Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.CourierTest
{
    public class IsCanTakeOrderTest
    {
        [Theory]
        [InlineData(5, 6)]
        [InlineData(10, 20)]
        [InlineData(30, 30)]
        [InlineData(60000, 60000)]
        [InlineData(10000, 10000000)]
        [InlineData(int.MaxValue, int.MaxValue)]
        public void WhenIsCourierCanTakingOrder_AndOrderVolumeLessOrEqualCourierTotalStoragePlaceVolume_AndCourierStoragePlaceNoHaveOrderId_ThenMethodShouldBeReturnTrue(int orderVolume, int storageVolume)
        {
            //Arragne
            var xOrder = 2;
            var yOrder = 1;
            var orderLocation = Location.Create(xOrder, yOrder);

            var xCourier = 4;
            var yCourier = 9;
            var currentCourierLocation = Location.Create(xCourier, yCourier);

            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation.Value, orderVolume);

            var courierSpeed = 2;
            var courierName = "Ivan Ivashov";
            var storagePlaceName = StoragePlace.Bag.Name;
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation.Value).Value;

            courier.CourierStoragePlaces.Clear();
            courier.AddNewStoragePlace(storagePlaceName, storageVolume);

            //Act
            var isCourierCanTakeOrderResult = courier.IsCanTakeOrder(order.Value);

            //Assert
            Assert.True(isCourierCanTakeOrderResult.Value);
            Assert.True(isCourierCanTakeOrderResult.IsSuccess);
            Assert.False(isCourierCanTakeOrderResult.IsFailure);
        }

        [Theory]
        [InlineData(12, 6)]
        [InlineData(30, 20)]
        [InlineData(276, 1)]
        [InlineData(90237, 60000)]
        [InlineData(10000000, 1000000)]
        [InlineData(int.MaxValue, int.MaxValue - 1)]
        public void WhenIsCourierCanTakingOrder_AndOrderVolumeGreaterTotalCourierStoragePlaceVolume_ThenMethodShouldBeReturnFalse(int orderVolume, int storageVolume)
        {
            //Arragne
            var xOrder = 3;
            var yOrder = 7;
            var orderLocation = Location.Create(xOrder, yOrder);

            var xCourier = 4;
            var yCourier = 9;
            var currentCourierLocation = Location.Create(xCourier, yCourier);

            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation.Value, orderVolume);

            var courierSpeed = 3;
            var courierName = "Dmitriy Letov";
            var storagePlaceName = StoragePlace.Bag.Name;

            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation.Value).Value;

            courier.CourierStoragePlaces.Clear();
            courier.AddNewStoragePlace(storagePlaceName, storageVolume);

            //Act
            var isCourierCanTakeOrderResult = courier.IsCanTakeOrder(order.Value);

            //Assert
            Assert.False(isCourierCanTakeOrderResult.Value);
            Assert.True(isCourierCanTakeOrderResult.IsSuccess);
            Assert.False(isCourierCanTakeOrderResult.IsFailure);
        }

        [Fact]
        public void WhenIsCourierCanTakingOrder_AndCourierStoragePlaceAlreadyHaveOrder_ThenMethodShouldBeReturnFalse()
        {
            //Arragne
            var xOrder = 3;
            var yOrder = 7;
            var orderLocation = Location.Create(xOrder, yOrder);

            var xCourier = 4;
            var yCourier = 9;
            var currentCourierLocation = Location.Create(xCourier, yCourier);

            var orderVolume = 6;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation.Value, orderVolume).Value;

            var courierSpeed = 4;
            var courierName = "Pavel Meshkov";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation.Value).Value;

            courier.TakeOrder(order);

            //Act
            var isCourierCanTakeOrderResult = courier.IsCanTakeOrder(order);

            //Assert
            Assert.False(isCourierCanTakeOrderResult.IsFailure);
            Assert.True(isCourierCanTakeOrderResult.IsSuccess);
            Assert.False(isCourierCanTakeOrderResult.Value);
        }

        [Fact]
        public void WhenIsCourierCanTakingOrder_AndCourierNoHaveStoragePlaces_ThenMethodShouldBeReturnFalse()
        {
            //Arragne
            var xOrder = 7;
            var yOrder = 2;
            var orderLocation = Location.Create(xOrder, yOrder);

            var xCourier = 9;
            var yCourier = 3;
            var currentCourierLocation = Location.Create(xCourier, yCourier);

            var orderVolume = 6;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation.Value, orderVolume);

            var courierSpeed = 4;
            var courierName = "Maxim Loshkov";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation.Value);
            courier.Value.CourierStoragePlaces.Clear();

            //Act
            var isCourierCanTakeOrderResult = courier.Value.IsCanTakeOrder(order.Value);

            //Assert
            Assert.True(courier.Value.CourierStoragePlaces.Count == 0);
            Assert.False(isCourierCanTakeOrderResult.IsFailure);
            Assert.True(isCourierCanTakeOrderResult.IsSuccess);
            Assert.False(isCourierCanTakeOrderResult.Value);
        }

        [Fact]
        public void WhenIsCourierCanTakingOrder_AndOrderNull_ThenMethodShouldBeReturnError()
        {
            //Arragne
            var xCourier = 9;
            var yCourier = 3;
            var currentCourierLocation = Location.Create(xCourier, yCourier);

            var courierSpeed = 4;
            var courierName = "Maxim Loshkov";

            Order order = null;
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation.Value).Value;

            //Act
            var isCourierCanTakeOrderResult = courier.IsCanTakeOrder(order);

            //Assert
            Assert.True(isCourierCanTakeOrderResult.IsFailure);
            Assert.False(isCourierCanTakeOrderResult.IsSuccess);
            Assert.True(isCourierCanTakeOrderResult.Error.InnerError.Code == "value.is.required");
            Assert.Throws<CSharpFunctionalExtensions.ResultFailureException<Error>>(() => isCourierCanTakeOrderResult.Value);
        }
    }
}