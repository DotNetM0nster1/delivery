using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate
{
    public class StoragePlaceTest
    {
        [Theory]
        [InlineData("", 1)]
        [InlineData(" ", 2)]
        [InlineData(null, 3)]
        public void WhenCreateStorage_AndStorageNameIsNullOrEmpty_ThenResultShouldBeFalase(
            string name, 
            int capacity)
        {
            //Act
            var storage = StoragePlace.CreateStorage(name, capacity);

            //Assert
            Assert.False(storage.IsSuccess);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-23)]
        [InlineData(-875)]
        [InlineData(int.MinValue)]
        public void WhenCreateStorage_AndStorageCapacityLessOrEqual0_ThenResultShouldBeFalase(
            int capacity)
        {
            //Arrange
            string name = "Some storage";

            //Act
            var storage = StoragePlace.CreateStorage(name, capacity);

            //Assert
            Assert.False(storage.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(20)]
        [InlineData(345)]
        [InlineData(812354)]
        [InlineData(int.MaxValue)]
        public void WhenCreateStorage_AndStorageCapacityGreater0_ThenResultShouldBeTrue(
            int capacity)
        {
            //Arrange
            string name = "Some storage";

            //Act
            var storage = StoragePlace.CreateStorage(name, capacity);

            //Assert
            Assert.True(storage.IsSuccess);
        }

        [Fact]
        public void WhenCheckIsOrderCorrectForPutInStorage_AndStorageAlreadyHaveOrder_AndStorageNotOverflow_ThenResultShouldBeFalse()
        {
            //Arrange
            var storageName = "Some storage";
            var storageMaxCapacity = 20;
            var itemsCount = 10;
            var orderId = Guid.NewGuid();
            var someStorage = StoragePlace.CreateStorage(storageName, storageMaxCapacity, orderId).Value;

            //Act
            var isOrderCorrectForInputInStorage = someStorage.IsOrderCorrectForPutInStorage(orderId, itemsCount);

            //Assert
            Assert.False(isOrderCorrectForInputInStorage.IsSuccess);
        }


        [Fact]
        public void WhenCheckIsOrderCorrectForPutInStorage_AndStorageAlreadyHaveOrder_AndStorageOverflow_ThenResultShouldBeFalse()
        {
            //Arrange
            var storageName = "Some storage";
            var storageMaxCapacity = 10;
            var itemsCount = 11;
            var orderId = Guid.NewGuid();
            var someStorage = StoragePlace.CreateStorage(storageName, storageMaxCapacity, orderId).Value;

            //Act
            var isOrderCorrectForInputInStorage = someStorage.IsOrderCorrectForPutInStorage(orderId, itemsCount);

            //Assert
            Assert.False(isOrderCorrectForInputInStorage.IsSuccess);
        }

        [Fact]
        public void WhenCheckIsOrderCorrectForPutInStorage_AndStorageNoHaveOrder_AndStorageNotOverflow_ThenResultShouldBeTrue()
        {
            //Arrange
            var storageName = "Some storage";
            var storageMaxCapacity = 10;
            var itemsCount = 5;
            var orderId = Guid.NewGuid();
            var someStorage = StoragePlace.CreateStorage(storageName, storageMaxCapacity).Value;

            //Act
            var isOrderCorrectForInputInStorage = someStorage.IsOrderCorrectForPutInStorage(orderId, itemsCount);

            //Assert
            Assert.True(isOrderCorrectForInputInStorage.IsSuccess);
        }

        [Fact]
        public void WhenCheckIsOrderCorrectForPutInStorage_AndStorageNoHaveOrder_AndStorageOverflow_ThenResultShouldBeFalse()
        {
            //Arrange
            var storageName = "Some storage";
            var storageMaxCapacity = 10;
            var itemsCount = 55;
            var orderId = Guid.NewGuid();
            var someStorage = StoragePlace.CreateStorage(storageName, storageMaxCapacity).Value;

            //Act
            var isOrderCorrectForInputInStorage = someStorage.IsOrderCorrectForPutInStorage(orderId, itemsCount);

            //Assert
            Assert.False(isOrderCorrectForInputInStorage.IsSuccess);
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
        public void WhenCheckIsOrderCorrectForPutInStorage_AndStorageNotOverflow_ThenResultShouldBeTrue(
            int storageMaxCapacity, 
            int itemsCount)
        {
            //Arrange
            var storageName = "Some storage";
            var orderId = Guid.NewGuid();
            var someStorage = StoragePlace.CreateStorage(storageName, storageMaxCapacity).Value;

            //Act
            var isOrderCorrectForInputInStorage = someStorage.IsOrderCorrectForPutInStorage(orderId, itemsCount);

            //Assert
            Assert.True(isOrderCorrectForInputInStorage.IsSuccess);
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
        public void WhenCheckIsOrderCorrectForPutInStorage_AndStorageOverflow_ThenResultShouldBeFalse(
            int storageMaxCapacity,
            int itemsCount)
        {
            //Arrange
            var storageName = "Some storage";
            var orderId = Guid.NewGuid();
            var someStorage = StoragePlace.CreateStorage(storageName, storageMaxCapacity).Value;

            //Act
            var isOrderCorrectForInputInStorage = someStorage.IsOrderCorrectForPutInStorage(orderId, itemsCount);

            //Assert
            Assert.False(isOrderCorrectForInputInStorage.IsSuccess);
        }

        [Fact]
        public void WhenPuttingOrderInStorage_AndOrderIsCorrect_ThenResultShouldBeTrue()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var itemsMaxCapacity = 20;
            var itemsCount = 10;
            var name = "Some name";
            var storage = StoragePlace.CreateStorage(name, itemsMaxCapacity).Value;

            //Act
            var isInputtingOrderResult = storage.PutOrderInStorage(orderId, itemsCount).IsSuccess;

            //Assert
            Assert.True(isInputtingOrderResult);
            Assert.True(storage.OrderId == orderId);
            Assert.True(storage.Name == name);
            Assert.True(storage.MaxCapacity == itemsMaxCapacity);
        }

        [Fact]
        public void WhenPuttingOrderInStorage_AndOrderAlreadyExistInStorage_ThenResultShouldBeFalse()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var itemsMaxCapacity = 60;
            var itemsCount = 12;
            var name = "Some name";
            var storage = StoragePlace.CreateStorage(name, itemsMaxCapacity, orderId).Value;

            //Act
            var isPuttingOrderResult = storage.PutOrderInStorage(orderId, itemsCount).IsSuccess;

            //Assert
            Assert.False(isPuttingOrderResult);
        }

        [Fact]
        public void WhenPuttingOrderInStorage_AndOrderOverflowStorage_ThenResultShouldBeFalse()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var itemsMaxCapacity = 5;
            var itemsCount = 88;
            var name = "Some name";
            var storage = StoragePlace.CreateStorage(name, itemsMaxCapacity).Value;

            //Act
            var isInputtingOrderResult = storage.PutOrderInStorage(orderId, itemsCount).IsSuccess;

            //Assert
            Assert.False(isInputtingOrderResult);
        }

        [Fact]
        public void WhenOutputOrderOfStorage_AndOrderIsExistInStorage_ThenResultSouldBeTrue()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var itemsMaxCapacity = 7;
            var itemsCount = 1;
            var name = "Some name";
            var storage = StoragePlace.CreateStorage(name, itemsMaxCapacity).Value;
            storage.PutOrderInStorage(orderId, itemsCount);

            //Act
            var isOutputtingOrderResult = storage.OutputOrderOfStorage().IsSuccess;

            //Assert
            Assert.True(isOutputtingOrderResult);
            Assert.True(storage.OrderId == null);
        }

        [Fact]
        public void WhenOutputOrderOfStorage_AndOrderNotExistInStorage_ThenResultSouldBeFalse()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var itemsMaxCapacity = 45;
            var name = "Some name";
            var storage = StoragePlace.CreateStorage(name, itemsMaxCapacity).Value;

            //Act
            var isOutputtingOrderResult = storage.OutputOrderOfStorage().IsSuccess;

            //Assert
            Assert.False(isOutputtingOrderResult);
        }
    }
}