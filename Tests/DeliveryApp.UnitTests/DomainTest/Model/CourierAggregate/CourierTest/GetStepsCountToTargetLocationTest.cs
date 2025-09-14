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
    public class GetStepsCountToTargetLocationTest
    {
        [Fact]
        public void WhenGettingStepsCountToTargetLocation_AndTargetLocationIsNull_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var xCourier = 4;
            var yCourier = 5;
            var currentCourierLocation = Location.Create(xCourier, yCourier).Value;

            var courierSpeed = 2;
            var courierName = "Semen Valodin";
            var courier = Courier.Create(courierSpeed, courierName, currentCourierLocation).Value;

            //Act
            var getStepsCountResult = courier.GetStepsCountToTargetLocation(null);

            //Assert
            Assert.True(getStepsCountResult.IsFailure);
            Assert.False(getStepsCountResult.IsSuccess);
            Assert.True(getStepsCountResult.Error.Code == "value.is.required");
        }

        [Theory]
        [InlineData(1, 1, 10, 2, 1, 10)]     
        [InlineData(1, 2, 10, 2, 2, 4.5)] 
        [InlineData(1, 3, 10, 2, 3, 3.3333333333333335)] 
        [InlineData(1, 4, 10, 2, 4, 2.75)] 
        [InlineData(1, 5, 10, 2, 5, 2.4)] 
        [InlineData(1, 6, 10, 2, 6, 2.1666666666666665)] 
        [InlineData(1, 7, 10, 2, 7, 2)] 
        [InlineData(1, 8, 10, 2, 8, 1.875)] 
        [InlineData(1, 9, 10, 2, 9, 1.7777777777777777)] 
        [InlineData(1, 10, 10, 2, 10, 1.7)]
        [InlineData(2, 1, 9, 2, 2, 4)] 
        [InlineData(2, 2, 9, 2, 3, 2.3333333333333335)] 
        [InlineData(2, 3, 9, 2, 4, 2)] 
        [InlineData(2, 4, 9, 2, 5, 1.8)] 
        [InlineData(2, 5, 9, 2, 6, 1.6666666666666667)] 
        [InlineData(2, 6, 9, 2, 7, 1.5714285714285714)] 
        [InlineData(2, 7, 9, 2, 8, 1.5)] 
        [InlineData(2, 8, 9, 2, 9, 1.4444444444444444)] 
        [InlineData(2, 9, 9, 2, 10, 1.4)]
        [InlineData(2, 10, 9, 2, 1, 15)]
        [InlineData(3, 1, 8, 2, 3, 2)] 
        [InlineData(3, 2, 8, 2, 4, 1.25)] 
        [InlineData(3, 3, 8, 2, 5, 1.2)] 
        [InlineData(3, 4, 8, 2, 6, 1.1666666666666667)] 
        [InlineData(3, 5, 8, 2, 7, 1.1428571428571428)] 
        [InlineData(3, 6, 8, 2, 8, 1.125)] 
        [InlineData(3, 7, 8, 2, 9, 1.1111111111111112)] 
        [InlineData(3, 8, 8, 2, 1, 11)] 
        [InlineData(3, 9, 8, 2, 2, 6)] 
        [InlineData(3, 10, 8, 2, 3, 4.333333333333333)]
        [InlineData(4, 1, 7, 2, 4, 1)] 
        [InlineData(4, 2, 7, 2, 5, 0.6)] 
        [InlineData(4, 3, 7, 2, 6, 0.6666666666666666)] 
        [InlineData(4, 4, 7, 2, 7, 0.7142857142857143)] 
        [InlineData(4, 5, 7, 2, 8, 0.75)] 
        [InlineData(4, 6, 7, 2, 9, 0.7777777777777778)] 
        [InlineData(4, 7, 7, 2, 10, 0.8)]
        [InlineData(4, 8, 7, 2, 3, 3)] 
        [InlineData(4, 9, 7, 2, 2, 5)] 
        [InlineData(4, 10, 7, 2, 1, 11)]
        [InlineData(5, 1, 6, 2, 5, 0.4)] 
        [InlineData(5, 2, 6, 2, 6, 0.16666666666666666)] 
        [InlineData(5, 3, 6, 2, 7, 0.2857142857142857)] 
        [InlineData(5, 4, 6, 2, 8, 0.375)] 
        [InlineData(5, 5, 6, 2, 9, 0.4444444444444444)] 
        [InlineData(5, 6, 6, 2, 10, 0.5)]
        [InlineData(5, 7, 6, 2, 4, 1.5)] 
        [InlineData(5, 8, 6, 2, 3, 2.3333333333333335)] 
        [InlineData(5, 9, 6, 2, 2, 4)] 
        [InlineData(5, 10, 6, 2, 1, 9)]
        [InlineData(6, 1, 5, 2, 6, 0.3333333333333333)] 
        [InlineData(6, 2, 5, 2, 7, 0.14285714285714285)] 
        [InlineData(6, 3, 5, 2, 8, 0.25)] 
        [InlineData(6, 4, 5, 2, 9, 0.3333333333333333)] 
        [InlineData(6, 5, 5, 2, 10, 0.4)]
        [InlineData(6, 6, 5, 2, 5, 1)] 
        [InlineData(6, 7, 5, 2, 4, 1.5)] 
        [InlineData(6, 8, 5, 2, 3, 2.3333333333333335)] 
        [InlineData(6, 9, 5, 2, 2, 4)] 
        [InlineData(6, 10, 5, 2, 1, 9)]
        [InlineData(7, 1, 4, 2, 7, 0.5714285714285714)] 
        [InlineData(7, 2, 4, 2, 8, 0.375)] 
        [InlineData(7, 3, 4, 2, 9, 0.4444444444444444)] 
        [InlineData(7, 4, 4, 2, 10, 0.5)]
        [InlineData(7, 5, 4, 2, 6, 1)]  
        [InlineData(7, 6, 4, 2, 5, 1.4)]   
        [InlineData(7, 7, 4, 2, 4, 2)]   
        [InlineData(7, 8, 4, 2, 3, 3)]   
        [InlineData(7, 9, 4, 2, 2, 5)]   
        [InlineData(7, 10, 4, 2, 1, 11)]  
        [InlineData(8, 1, 3, 2, 8, 0.75)]   
        [InlineData(8, 2, 3, 2, 9, 0.5555555555555556)]   
        [InlineData(8, 3, 3, 2, 10, 0.6)]  
        [InlineData(8, 4, 3, 2, 7, 1)]   
        [InlineData(8, 5, 3, 2, 6, 1.3333333333333333)]   
        [InlineData(8, 6, 3, 2, 5, 1.8)]   
        [InlineData(8, 7, 3, 2, 4, 2.5)]   
        [InlineData(8, 8, 3, 2, 3, 3.6666666666666665)]   
        [InlineData(8, 9, 3, 2, 2, 6)]   
        [InlineData(8, 10, 3, 2, 1, 13)]  
        [InlineData(9, 1, 2, 2, 9, 0.8888888888888888)]   
        [InlineData(9, 2, 2, 2, 10, 0.7)]  
        [InlineData(9, 3, 2, 2, 8, 1)]   
        [InlineData(9, 4, 2, 2, 7, 1.2857142857142858)]   
        [InlineData(9, 5, 2, 2, 6, 1.6666666666666667)]   
        [InlineData(9, 6, 2, 2, 5, 2.2)]   
        [InlineData(9, 7, 2, 2, 4, 3)]   
        [InlineData(9, 8, 2, 2, 3, 4.333333333333333)]   
        [InlineData(9, 9, 2, 2, 2, 7)]   
        [InlineData(9, 10, 2, 2, 1, 15)]  
        [InlineData(10, 1, 1, 2, 10, 1)] 
        [InlineData(10, 2, 1, 2, 9, 1)]  
        [InlineData(10, 3, 1, 2, 8, 1.25)]  
        [InlineData(10, 4, 1, 2, 7, 1.5714285714285714)]  
        [InlineData(10, 5, 1, 2, 6, 2)]  
        [InlineData(10, 6, 1, 2, 5, 2.6)]  
        [InlineData(10, 7, 1, 2, 4, 3.5)]  
        [InlineData(10, 8, 1, 2, 3, 5)]  
        [InlineData(10, 9, 1, 2, 2, 8)]  
        [InlineData(10, 10, 1, 2, 1, 17)]
        public void WhenGettingStepsCountToTargetLocation_AndInputValuesCorrect_ThenMethodShouldBeReturnStepsCount(int xCurrent, int yCurrent, int xTarget, int yTarget, int courierSpeed, double result)
        {
            //Arrange
            var currentLocation = Location.Create(xCurrent, yCurrent);
            var targetLocation = Location.Create(xTarget, yTarget);

            var courierName = "Some Name";
            var courier = Courier.Create(courierSpeed, courierName, currentLocation.Value);

            //Act
            var courierStepsByTargetLocationResult = courier.Value.GetStepsCountToTargetLocation(targetLocation.Value);

            //Assert
            Assert.True(courierStepsByTargetLocationResult.IsSuccess);
            Assert.False(courierStepsByTargetLocationResult.IsFailure);
            Assert.True(courierStepsByTargetLocationResult.Value == result);
        }
    }
}