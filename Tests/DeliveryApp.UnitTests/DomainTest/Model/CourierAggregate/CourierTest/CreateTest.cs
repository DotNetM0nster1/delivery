using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Primitives;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.CourierTest
{
    public class CreateCourierTest
    {
        [Theory]
        [InlineData(10, "", 6, 9)]
        [InlineData(3, " ", 5, 1)]
        [InlineData(5, " ", -345, -3)]
        [InlineData(8, null, 7, 2)]
        [InlineData(1, "Kirill Nesterov", -2, 0)]
        [InlineData(-1, "Alexey Valodin", 4, 8)]
        [InlineData(-2134, " ", 3, 6)]
        [InlineData(int.MinValue, "", 2, 5)]
        [InlineData(7, null, 0, 0)]
        public void WhenCreatingCourer_AndSomeOfValuesIsNotCorrect_ThenMethodShouldBeRetturnError(int courierSpeed, string courierName, int x, int y)
        {
            //Arrange
            var currentCourierLocation = Location.Create(x, y);

            //Act
            var createCourierResult = Courier.Create(courierSpeed, courierName, currentCourierLocation.IsFailure ? null : currentCourierLocation.Value);

            //Assert
            Assert.True(createCourierResult.IsFailure);
            Assert.False(createCourierResult.IsSuccess);
            Assert.Throws<CSharpFunctionalExtensions.ResultFailureException<Error>>(() => createCourierResult.Value);
        }

        [Fact]
        public void WhenCreatingCourer_AndValuesIsCorrect_ThenMethodShouldBeCreateCourier()
        {
            //Arrange
            var x = 1;
            var y = 2;
            var currentCourierLocation = Location.Create(x, y);

            var courierSpeed = 1;
            var courierName = "Sergey Zakotskov";

            //Act
            var createCourierResult = Courier.Create(courierSpeed, courierName, currentCourierLocation.Value);

            //Assert
            Assert.True(createCourierResult.IsSuccess);
            Assert.False(createCourierResult.IsFailure);
            Assert.True(createCourierResult.Value.Name == courierName);
            Assert.True(createCourierResult.Value.Speed == courierSpeed);
            Assert.True(createCourierResult.Value.Location == currentCourierLocation.Value);
        }
    }
}