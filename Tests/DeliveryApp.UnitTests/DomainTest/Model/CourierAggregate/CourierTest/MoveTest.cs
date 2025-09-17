using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.CourierAggregate.CourierTest
{
    public class MoveTest
    {
        [Theory]
        [InlineData(1, 1, 10, 1, 10, 10, 1)] 
        [InlineData(1, 2, 10, 2, 3, 4, 2)]
        [InlineData(1, 3, 10, 3, 5, 6, 3)]
        [InlineData(1, 4, 10, 4, 3, 4, 4)]
        [InlineData(1, 5, 10, 5, 10, 10, 5)]
        [InlineData(1, 6, 10, 6, 9, 10, 6)]
        [InlineData(1, 7, 10, 7, 3, 4, 7)]
        [InlineData(1, 8, 10, 8, 9, 10, 8)]
        [InlineData(1, 9, 10, 9, 6, 7, 9)]
        [InlineData(1, 10, 10, 10, 9, 10, 10)]
        [InlineData(2, 1, 9, 2, 3, 5, 1)]
        [InlineData(2, 2, 9, 3, 5, 7, 2)]
        [InlineData(2, 3, 9, 4, 8, 9, 4)]
        [InlineData(2, 4, 9, 5, 1, 3, 4)]
        [InlineData(2, 5, 9, 6, 1, 3, 5)]
        [InlineData(2, 6, 9, 7, 2, 4, 6)]
        [InlineData(2, 7, 9, 8, 10, 9, 8)]
        [InlineData(2, 8, 9, 9, 4, 6, 8)]
        [InlineData(2, 9, 9, 10, 10, 9, 10)]
        [InlineData(2, 10, 9, 1, 5, 7, 10)]
        [InlineData(3, 1, 8, 3, 4, 7, 1)]
        [InlineData(3, 2, 8, 4, 3, 6, 2)]
        [InlineData(3, 3, 8, 5, 9, 8, 5)]
        [InlineData(3, 4, 8, 6, 5, 8, 4)]
        [InlineData(3, 5, 8, 7, 5, 8, 5)]
        [InlineData(3, 6, 8, 8, 3, 6, 6)]
        [InlineData(3, 7, 8, 9, 2, 5, 7)]
        [InlineData(3, 8, 8, 10, 5, 8, 8)]
        [InlineData(3, 9, 8, 1, 8, 8, 6)]
        [InlineData(3, 10, 8, 2, 9, 8, 6)]
        [InlineData(4, 1, 7, 3, 9, 7, 3)]
        [InlineData(4, 2, 7, 4, 2, 6, 2)]
        [InlineData(4, 3, 7, 5, 9, 7, 5)]
        [InlineData(4, 4, 7, 6, 10, 7, 6)]
        [InlineData(4, 5, 7, 7, 4, 7, 6)]
        [InlineData(4, 6, 7, 8, 4, 7, 7)]
        [InlineData(4, 7, 7, 9, 5, 7, 9)]
        [InlineData(4, 8, 7, 10, 9, 7, 10)]
        [InlineData(4, 9, 7, 1, 6, 7, 6)]
        [InlineData(4, 10, 7, 2, 1, 5, 10)]
        [InlineData(5, 1, 6, 4, 7, 6, 4)]
        [InlineData(5, 2, 6, 5, 10, 6, 5)]
        [InlineData(5, 3, 6, 6, 2, 6, 4)]
        [InlineData(5, 4, 6, 7, 3, 6, 6)]
        [InlineData(5, 5, 6, 8, 7, 6, 8)]
        [InlineData(5, 6, 6, 9, 6, 6, 9)]
        [InlineData(5, 7, 6, 10, 7, 6, 10)]
        [InlineData(5, 8, 6, 1, 9, 6, 1)]
        [InlineData(5, 9, 6, 2, 9, 6, 2)]
        [InlineData(5, 10, 6, 3, 1, 6, 10)]
        [InlineData(6, 1, 5, 5, 4, 5, 4)]
        [InlineData(6, 2, 5, 6, 7, 5, 6)]
        [InlineData(6, 3, 5, 7, 4, 5, 6)]
        [InlineData(6, 4, 5, 8, 4, 5, 7)]
        [InlineData(6, 5, 5, 9, 4, 5, 8)]
        [InlineData(6, 6, 5, 10, 1, 5, 6)]
        [InlineData(6, 7, 5, 1, 8, 5, 1)]
        [InlineData(6, 8, 5, 2, 2, 5, 7)]
        [InlineData(6, 9, 5, 3, 7, 5, 3)]
        [InlineData(6, 10, 5, 4, 6, 5, 5)]
        [InlineData(7, 1, 4, 6, 2, 5, 1)]
        [InlineData(7, 2, 4, 7, 2, 5, 2)]
        [InlineData(7, 3, 4, 8, 5, 4, 5)]
        [InlineData(7, 4, 4, 9, 5, 4, 6)]
        [InlineData(7, 5, 4, 10, 1, 6, 5)]
        [InlineData(7, 6, 4, 1, 9, 4, 1)]
        [InlineData(7, 7, 4, 2, 10, 4, 2)]
        [InlineData(7, 8, 4, 3, 9, 4, 3)]
        [InlineData(7, 9, 4, 4, 1, 6, 9)]
        [InlineData(7, 10, 4, 5, 8, 4, 5)]
        [InlineData(8, 1, 3, 7, 3, 5, 1)]
        [InlineData(8, 2, 3, 8, 6, 3, 3)]
        [InlineData(8, 3, 3, 9, 3, 5, 3)]
        [InlineData(8, 4, 3, 10, 10, 3, 9)]
        [InlineData(8, 5, 3, 1, 1, 7, 5)]
        [InlineData(8, 6, 3, 2, 9, 3, 2)]
        [InlineData(8, 7, 3, 3, 4, 4, 7)]
        [InlineData(8, 8, 3, 4, 8, 3, 5)]
        [InlineData(8, 9, 3, 5, 10, 3, 5)]
        [InlineData(8, 10, 3, 6, 3, 5, 10)]
        [InlineData(9, 1, 2, 8, 4, 5, 1)]
        [InlineData(9, 2, 2, 9, 4, 5, 2)]
        [InlineData(9, 3, 2, 10, 2, 7, 3)]
        [InlineData(9, 4, 2, 1, 1, 8, 4)]
        [InlineData(9, 5, 2, 2, 4, 5, 5)]
        [InlineData(9, 6, 2, 3, 2, 7, 6)]
        [InlineData(9, 7, 2, 4, 8, 2, 6)]
        [InlineData(9, 8, 2, 5, 8, 2, 7)]
        [InlineData(9, 9, 2, 6, 4, 5, 9)]
        [InlineData(9, 10, 2, 7, 9, 2, 8)]
        [InlineData(10, 1, 1, 9, 6, 4, 1)]
        [InlineData(10, 2, 1, 10, 4, 6, 2)]
        [InlineData(10, 3, 1, 1, 10, 1, 2)]
        [InlineData(10, 4, 1, 2, 6, 4, 4)]
        [InlineData(10, 5, 1, 3, 10, 1, 4)]
        [InlineData(10, 6, 1, 4, 2, 8, 6)]
        [InlineData(10, 7, 1, 5, 4, 6, 7)]
        [InlineData(10, 8, 1, 6, 8, 2, 8)]
        [InlineData(10, 9, 1, 7, 3, 7, 9)]
        [InlineData(10, 10, 1, 8, 5, 5, 10)]

        public void WhenMoving_AndCurrentLocationIsCorrect_AndTargetLocationsIsCorrect_ThenMethodShouldBeChangeCourierLocation(int xCurrent, int yCurrent, int xTarget, int yTarget, int courierSpeed, int resultX, int resultY) 
        {
            //Arrange
            var currentLocation = Location.Create(xCurrent, yCurrent).Value;
            var targetLocation = Location.Create(xTarget, yTarget).Value;

            var courierName = "FirstName LastName";
            var courier = Courier.Create(courierSpeed, courierName, currentLocation).Value;

            //Act
            var moveResult = courier.Move(targetLocation);

            //Assert
            Assert.True(moveResult.IsSuccess);
            Assert.False(moveResult.IsFailure);
            Assert.True(courier.CourierLocation.X == resultX && courier.CourierLocation.Y == resultY);
        }
    }
}