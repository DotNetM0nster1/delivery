using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Primitives;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.CourierTest
{

    public class AddNewStoragePlaceForCourierTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("                                   ")]
        public void WhenAddingNewStoragePlaceForCourier_AndStoragePlaceNameIsNotCorrect_ThenMethodShouldBeReturnError(string storagePlaceName)
        {
            //Arrange
            var x = 8;
            var y = 9;
            var currentCourierLocation = Location.Create(x, y).Value;

            var courierSpeed = 4;
            var maxStoragePlaceVolume = 20;
            var courierName = "Artem Garin";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation).Value;

            //Act
            var addNewStoragePlaseResult = courier.AddNewStoragePlace(storagePlaceName, maxStoragePlaceVolume);

            //Assert
            Assert.True(addNewStoragePlaseResult.IsFailure);
            Assert.False(addNewStoragePlaseResult.IsSuccess);
            Assert.Throws<ResultFailureException<Error>>(() => addNewStoragePlaseResult.Value);
            Assert.True(addNewStoragePlaseResult.Error.InnerError.Code == "value.is.invalid");
        }

        [Theory]
        [InlineData(1, 4, 2, "David Torosyan", 20, "bag")]
        [InlineData(6, 2, 3, "Pavel Marasenkov", 20, "termobag")]
        [InlineData(9, 7, 10, "Artem Karatenko", 200, "cartrunk")]
        [InlineData(1, 7, 4, "Aleksandr Pavlenko", 20, "bag")]
        [InlineData(2, 8, 3, "Sergey Prostakov", 20, "bag")]
        [InlineData(3, 1, 6, "Vova Ponatov", 20, "bag")]
        [InlineData(2, 4, 2, "Dmitriy Ivashov", 20, "bag")]
        [InlineData(1, 1, 5, "Misha Mankov", 20, "bag")]
        [InlineData(6, 8, 1, "Yarik Guskov", 1, "pocket")]
        [InlineData(1, 9, 2, "Nikita Svetin", 20, "bag")]
        [InlineData(7, 7, 9, "Egor Chernikov", 20, "bikerack")]
        [InlineData(5, 1, 8, "Roman Ruchkin", 20, "bag")]
        [InlineData(1, 3, 4, "Yuriy Aksenov", 10, "suitcase")]
        [InlineData(3, 7, 7, "Nikolay Morozin", 20, "bag")]
        public void WhenAddingNewStoragePlaceForCourier_AndStoragePlaceIsCorrect_ThenMethodShouldBeAddNewStoragePlace(int x, int y, int courierSpeed, string courierName, int storageTotalVolume, string storageName)
        {
            //Arrange
            var currentCourierLocation = Location.Create(x, y).Value;
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation).Value;

            //Act
            var addNewStoragePlaseResult = courier.AddNewStoragePlace(storageName, storageTotalVolume);

            //Assert
            Assert.True(addNewStoragePlaseResult.IsSuccess);
            Assert.False(addNewStoragePlaseResult.IsFailure);
            Assert.True(courier.StoragePlaces.Count == 1 + 1);
            Assert.True(courier.StoragePlaces.Capacity == 1 + 1);
        }
    }
}