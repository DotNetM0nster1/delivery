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
    public class RemoveOrderOfStorageTest
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
            storagePlace.AddOrderInStoragePlace(order);

            //Act
            var removeOrderOfStorageResult = storagePlace.RemoveOrderOfStorage();

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
            var removeOrderOfStorageResult = storagePlace.RemoveOrderOfStorage();

            //Assert
            Assert.False(removeOrderOfStorageResult.IsSuccess);
            Assert.True(removeOrderOfStorageResult.IsFailure);
            Assert.True(removeOrderOfStorageResult.Error.Code == "value.is.invalid");
        }
    }
}