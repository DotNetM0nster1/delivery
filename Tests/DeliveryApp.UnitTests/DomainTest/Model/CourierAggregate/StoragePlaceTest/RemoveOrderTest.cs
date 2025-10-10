using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.StoragePlaceTest
{
    public class RemoveOrderTest
    {
        [Fact]
        public void WhenRemovingOrderOfStorage_AndStoragePlaceHaveOrder_ThenMethodShouldBeRemoveOrderOfStorage()
        {
            //Arrange
            var x = 7;
            var y = 2;
            var orderLocation = Location.Create(x, y).Value;

            var basketId = Guid.NewGuid();
            var currentStoragePlaceVolume = 1;
            var order = Order.Create(basketId, orderLocation, currentStoragePlaceVolume).Value;

            var maxStoragePlaceVolume = 7;
            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;
            storagePlace.AddOrder(order);

            //Act
            var removeOrderOfStorageResult = storagePlace.RemoveOrder();

            //Assert
            Assert.False(removeOrderOfStorageResult.IsFailure);
            Assert.True(removeOrderOfStorageResult.IsSuccess);
            Assert.True(storagePlace.OrderId == null);
        }

        [Fact]
        public void WhenRemovingOrderOfStorage_AndStoragePlaceNoHaveOrder_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var maxStoragePlaceVolume = 45;
            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;

            //Act
            var removeOrderOfStorageResult = storagePlace.RemoveOrder();

            //Assert
            Assert.False(removeOrderOfStorageResult.IsSuccess);
            Assert.True(removeOrderOfStorageResult.IsFailure);
            Assert.True(removeOrderOfStorageResult.Error.Code == "value.is.invalid");
        }
    }
}