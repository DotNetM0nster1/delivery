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
    public class IsOrderCorrectForAddTest
    {
        [Fact]
        public void WhenIsOrderCorrectForAdding_AndStoragePlaceAlreadyHaveOrder_ThenMethodShouldBeReturnFalse()
        {
            //Arrange
            var x = 9;
            var y = 8;
            var orderLocation = Location.Create(x, y).Value;

            var orderVolume = 10;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            var maxStoragePlaceVolume = 20;
            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;
            storagePlace.AddOrder(order);

            //Act
            var isOrderCorrectForAddResult = storagePlace.IsOrderCorrectForAdd(order);

            //Assert
            Assert.False(isOrderCorrectForAddResult.IsFailure);
            Assert.True(isOrderCorrectForAddResult.IsSuccess);
            Assert.False(isOrderCorrectForAddResult.Value);
        }


        [Fact]
        public void WhenIsOrderCorrectForAdding_AndStorageAlreadyHaveOrder_AndStorageOverflow_ThenMethodShouldBeReturnFalse()
        {
            //Arrange
            var x = 1;
            var y = 2;
            var orderLocation = Location.Create(x, y).Value;

            var orderVolume = 11;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            var maxStoragePlaceVolume = 10;
            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;
            storagePlace.AddOrder(order);

            //Act
            var isOrderCorrectForAddResult = storagePlace.IsOrderCorrectForAdd(order);

            //Assert
            Assert.False(isOrderCorrectForAddResult.IsFailure);
            Assert.True(isOrderCorrectForAddResult.IsSuccess);
            Assert.False(isOrderCorrectForAddResult.Value);
        }

        [Fact]
        public void WhenIsOrderCorrectForAdding_AndStorageNoHaveOrder_AndStorageNotOverflow_ThenMethodSouldBeReturnTrue()
        {
            //Arrange
            var x = 3;
            var y = 7;
            var orderLocation = Location.Create(x, y).Value;

            var maxStoragePlaceVolume = 10;
            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;

            var orderVolume = 5;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var isOrderCorrectForAddResult = storagePlace.IsOrderCorrectForAdd(order);

            //Assert
            Assert.False(isOrderCorrectForAddResult.IsFailure);
            Assert.True(isOrderCorrectForAddResult.IsSuccess);
            Assert.True(isOrderCorrectForAddResult.Value);
        }

        [Fact]
        public void WhenIsOrderCorrectForAdding_AndStorageNoHaveOrder_AndStorageOverflow_ThenMethodSouldBeReturnFalse()
        {
            //Arrange
            var x = 5;
            var y = 2;
            var orderLocation = Location.Create(x, y).Value;

            var maxStoragePlaceVolume = 10;
            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;

            var orderVolume = 55;
            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var isOrderCorrectForAddResult = storagePlace.IsOrderCorrectForAdd(order);

            //Assert
            Assert.False(isOrderCorrectForAddResult.IsFailure);
            Assert.True(isOrderCorrectForAddResult.IsSuccess);
            Assert.False(isOrderCorrectForAddResult.Value);
        }

        [Theory]
        [InlineData(20, 3)]
        [InlineData(20, 15)]
        [InlineData(20, 20)]
        [InlineData(25, 6)]
        [InlineData(25, 17)]
        [InlineData(25, 25)]
        [InlineData(100, 97)]
        [InlineData(100, 100)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue, int.MaxValue - 31)]
        [InlineData(int.MaxValue, int.MaxValue - 8421)]
        public void WhenIsOrderCorrectForAdding_AndStoragePlaceVolumeGreaterWhenOrderVolume_ThenResultShouldBeTrue(int maxStoragePlaceVolume, int orderVolume)
        {
            //Arrange
            var x = 6;
            var y = 1;
            var orderLocation = Location.Create(x, y).Value;

            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;

            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var isOrderCorrectForAddResult = storagePlace.IsOrderCorrectForAdd(order);

            //Assert
            Assert.False(isOrderCorrectForAddResult.IsFailure);
            Assert.True(isOrderCorrectForAddResult.IsSuccess);
            Assert.True(isOrderCorrectForAddResult.IsSuccess);
        }

        [Theory]
        [InlineData(20, 21)]
        [InlineData(20, 36)]
        [InlineData(20, 87)]
        [InlineData(25, 26)]
        [InlineData(25, 91)]
        [InlineData(25, 149)]
        [InlineData(100, 101)]
        [InlineData(100, 634)]
        [InlineData(int.MaxValue - 1, int.MaxValue)]
        [InlineData(int.MaxValue - 100, int.MaxValue)]
        [InlineData(int.MaxValue - 1000, int.MaxValue)]
        public void WhenIsOrderCorrectForAdding_AndStoragePlaceLessOrEqualOrderVolme_ThenMethodShouldBeReturnFalse(int maxStoragePlaceVolume, int orderVolume)
        {
            //Arrange
            var x = 9;
            var y = 5;
            var orderLocation = Location.Create(x, y).Value;

            var storagePlaceName = "Some name";
            var storagePlace = StoragePlace.Create(storagePlaceName, maxStoragePlaceVolume).Value;

            var basketId = Guid.NewGuid();
            var order = Order.Create(basketId, orderLocation, orderVolume).Value;

            //Act
            var isOrderCorrectForAddResult = storagePlace.IsOrderCorrectForAdd(order);

            //Assert
            Assert.False(isOrderCorrectForAddResult.IsFailure);
            Assert.True(isOrderCorrectForAddResult.IsSuccess);
            Assert.False(isOrderCorrectForAddResult.Value);
        }
    }
}