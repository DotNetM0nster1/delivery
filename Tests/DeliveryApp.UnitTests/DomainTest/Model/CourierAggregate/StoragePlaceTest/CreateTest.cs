using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.StoragePlaceTest
{
    public class CreateTest
    {
        [Theory]
        [InlineData("", 1)]
        [InlineData(" ", 2)]
        [InlineData(null, 3)]
        public void WhenCreatingStoragePlace_AndStorageNameIsNullOrEmptyOrWhiteSpace_ThenMethodShouldBeReturnError(string storagePlaceName, int storagePlaceVolume)
        {
            //Act
            var createStoragePlaceResult = StoragePlace.Create(storagePlaceName, storagePlaceVolume);

            //Assert
            Assert.False(createStoragePlaceResult.IsSuccess);
            Assert.True(createStoragePlaceResult.IsFailure);
            Assert.True(createStoragePlaceResult.Error.Code == "value.is.invalid");
            Assert.Throws<CSharpFunctionalExtensions.ResultFailureException<Error>>(() => createStoragePlaceResult.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-23)]
        [InlineData(-875)]
        [InlineData(int.MinValue)]
        public void WhenCreatingStoragePlace_AndStorageCapacityLessOrEqual0_ThenMethodShouldBeReturnError(int storagePlaceVolume)
        {
            //Arrange
            var storagePlaceName = "Some storage";

            //Act
            var createStoragePlaceResult = StoragePlace.Create(storagePlaceName, storagePlaceVolume);

            //Assert
            Assert.False(createStoragePlaceResult.IsSuccess);
            Assert.True(createStoragePlaceResult.IsFailure);
            Assert.True(createStoragePlaceResult.Error.Code == "value.is.invalid");
            Assert.Throws<CSharpFunctionalExtensions.ResultFailureException<Error>>(() => createStoragePlaceResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(20)]
        [InlineData(345)]
        [InlineData(812354)]
        [InlineData(int.MaxValue)]
        public void WhenCreatingStoragePlace_AndStoragePlaceIsCorrect_ThenMethodShouldBeCreateStoragePlace(int storagePlaceVolume)
        {
            //Arrange
            var storagePlaceName = "Some storage";

            //Act
            var createStoragePlaceResult = StoragePlace.Create(storagePlaceName, storagePlaceVolume);

            //Assert
            Assert.True(createStoragePlaceResult.IsSuccess);
            Assert.True(createStoragePlaceResult.Value.Name == storagePlaceName);
            Assert.True(createStoragePlaceResult.Value.TotalVolume == storagePlaceVolume);
        }

        [Fact]
        public void WhenCreatingStoragePlace_AndStoragePlaceIsCorrect_AndStoragePlaceHaveOrderId_ThenMethodShouldBeCreateStoragePlace()
        {
            //Arrange
            var storagePlaceVolume = 22;
            var storagePlaceName = "Some storage";

            //Act
            var createStoragePlaceResult = StoragePlace.Create(storagePlaceName, storagePlaceVolume);

            //Assert
            Assert.True(createStoragePlaceResult.IsSuccess);
            Assert.True(createStoragePlaceResult.Value.Name == storagePlaceName);
            Assert.True(createStoragePlaceResult.Value.TotalVolume == storagePlaceVolume);
        }
    }
}