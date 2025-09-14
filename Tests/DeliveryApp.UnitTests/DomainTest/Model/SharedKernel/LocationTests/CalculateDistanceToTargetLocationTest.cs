using DeliveryApp.Core.Domain.Model.SharedKernel;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.SharedKernel.LocationTests
{
    public class CalculateDistanceToTargetLocationTest
    {
        [Theory]
        [InlineData(10, 10, 1, 1)]
        [InlineData(1, 1, 10, 10)]
        [InlineData(10, 1, 1, 10)]
        [InlineData(1, 10, 10, 1)]
        public void WhenCalculatingDistanceToTargetLocation_AndThisDioganalMoving_ThenMethodShouldBeReturn18(int currentX, int currentY, int targetX, int targetY)
        {
            //Arragne 
            var currentLocation = Location.Create(currentX, currentY);
            var targetLocation = Location.Create(targetX, targetY);

            //Act
            var stepsCountResult = currentLocation.Value.CalculateDistanceToTargetLocation(targetLocation.Value); 

            //Assert
            Assert.Equal(18, stepsCountResult);
        }

        [Theory]
        [InlineData(1, 5, 10, 5)]
        [InlineData(10, 5, 1, 5)]
        public void WhenCalculatingDistanceToTargetLocation_AndThisHorizontalMoving_ThenMethodShouldBeReturn9(int currentX, int currentY, int targetX, int targetY)
        {
            //Arragne 
            var currentLocation = Location.Create(currentX, currentY);
            var targetLocation = Location.Create(targetX, targetY);

            //Act
            var stepsCountResult = currentLocation.Value.CalculateDistanceToTargetLocation(targetLocation.Value);

            //Assert
            Assert.Equal(9, stepsCountResult);
        }

        [Theory]
        [InlineData(1, 1, 1, 10)]
        [InlineData(1, 10, 1, 1)]
        public void WhenCalculatingDistanceToTargetLocation_AndThisVerticallMoving_ThenMethodShouldBeReturn9(int currentX, int currentY, int targetX, int targetY)
        {
            //Arragne 
            var currentLocation = Location.Create(currentX, currentY);
            var targetLocation = Location.Create(targetX, targetY);

            //Act
            var stepsCountResult = currentLocation.Value.CalculateDistanceToTargetLocation(targetLocation.Value);

            //Assert
            Assert.Equal(9, stepsCountResult);
        }

        [Fact]
        public void WhenCalculatingDistanceToTargetLocation_AndCurrentLocationX9Y4_AndTargetLocationX6Y8_ThenMethodShouldBeReturn7()
        {
            //Arragne
            var currentX = 9;
            var currentY = 4;
            var targetX = 6;
            var targetY = 8;

            var currentLocation = Location.Create(currentX, currentY);
            var targetLocation = Location.Create(targetX, targetY);

            //Act
            var stepsCountResult = currentLocation.Value.CalculateDistanceToTargetLocation(targetLocation.Value);

            //Assert
            Assert.Equal(7, stepsCountResult);
        }

        [Fact]
        public void WhenCalculatingDistanceToTargetLocation_AndCurrentLocationX3Y1_AndTargetLocationX5Y9_ThenMethodShouldBeReturn10()
        {
            //Arragne
            var currentX = 3;
            var currentY = 1;
            var targetX = 5;
            var targetY = 9;

            var currentLocation = Location.Create(currentX, currentY);
            var targetLocation = Location.Create(targetX, targetY);

            //Act
            var stepsCountResult = currentLocation.Value.CalculateDistanceToTargetLocation(targetLocation.Value);

            //Assert
            Assert.Equal(10, stepsCountResult);
        }

        [Fact]
        public void WhenCalculatingDistanceToTargetLocation_AndCurrentLocationX8Y4_AndTargetLocationX9Y4_ThenMethodSouldBeReturn1()
        {
            //Arragne
            var currentX = 8;
            var currentY = 4;
            var targetX = 9;
            var targetY = 4;

            var currentLocation = Location.Create(currentX, currentY);
            var targetLocation = Location.Create(targetX, targetY);

            //Act
            var stepsCountResult = currentLocation.Value.CalculateDistanceToTargetLocation(targetLocation.Value);

            //Assert
            Assert.Equal(1, stepsCountResult);
        }

        [Fact]
        public void WhenCalculatingDistanceToTargetLocation_AndCurrentLocationX6Y6_AndTargetLocationX6Y7_ThenMethodShouldBeReturn1()
        {
            //Arragne
            var currentX = 6;
            var currentY = 6;
            var targetX = 6;
            var targetY = 7;

            var currentLocation = Location.Create(currentX, currentY);
            var targetLocation = Location.Create(targetX, targetY);

            //Act
            var stepsCountResult = currentLocation.Value.CalculateDistanceToTargetLocation(targetLocation.Value);

            //Assert
            Assert.Equal(1, stepsCountResult);
        }
    }
}