using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.StoragePlaceTest
{
    public class AddOrderInStoragePlaceTest
    {
        [Fact]
        public void WhenAddingOrderInStoragePlace_AndOrderIsCorrect_ThenMethodShouldBeAddOrderInStoragePlace()
        {
            //Arrange
            var x = 5;
            var y = 7;
            var orderLocation = Location.Create(x, y).Value;

            var maxStoragePlaceVolume = 20;
            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;

            var orderVolume = 10;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var addOrderResult = storagePlace.AddOrderInStoragePlace(order);

            //Assert
            Assert.True(addOrderResult.IsSuccess);
            Assert.True(storagePlace.OrderId == order.Id);
            Assert.True(storagePlace.Name == storagePlaceName);
            Assert.True(storagePlace.TotalVolume == maxStoragePlaceVolume);
        }

        [Fact]
        public void WhenAddingOrderInStoragePlace_AndStoragePlaceAlreadyHaveAnotherOrder_ThenMethodShouldntBeAddOrderInStoragePlace()
        {
            //Arrange
            var x = 8;
            var y = 1;
            var orderLocation = Location.Create(x, y).Value;

            var maxStoragePlaceVolume = 60;
            var storagePlaceName = "Some name";
            var basketId = Guid.NewGuid();
            var storage = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume, basketId).Value;

            var orderVolume = 12;
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var addOrderResult = storage.AddOrderInStoragePlace(order);

            //Assert
            Assert.False(addOrderResult.IsSuccess);
            Assert.True(addOrderResult.IsFailure);
            Assert.True(addOrderResult.Error.Code == "order.null.or.order.volume.is.not.correct.or.storage.place.already.have.order");
        }

        [Fact]
        public void WhenAddingOrderInStoragePlace_AndOrderVolumeGreaterWhenStoragePlaceVolume_ThenMethodShouldntBeAddOrderInStoragePlace()
        {
            //Arrange
            var x = 4;
            var y = 6;
            var orderLocation = Location.Create(x, y).Value;

            var maxStoragePlaceVolume = 5;
            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;

            var orderVolume = 88;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var addOrderResult = storagePlace.AddOrderInStoragePlace(order);

            //Assert
            Assert.False(addOrderResult.IsSuccess);
            Assert.True(addOrderResult.IsFailure);
            Assert.True(addOrderResult.Error.Code == "order.null.or.order.volume.is.not.correct.or.storage.place.already.have.order");
        }
    }
}