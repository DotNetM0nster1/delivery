using DeliveryApp.Core.Domain.Model.SharedKernel;
using System.Collections.Generic;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.SharedKernel.LocationTests
{
    public class CreateRandomLocationTest
    {
        [Fact]
        public void WhenCreatingRandomLocation_ThenResultShouldBeTrue()
        {
            List<bool> isSuccess = [];
            int counter = 100000;

            while (counter > 0)
            {
                //Act
                var randomLocation = Location.CreateRandomLocation();
                isSuccess.Add(randomLocation.IsSuccess);
                counter--;
            }

            //Assert
            Assert.DoesNotContain(false, isSuccess);
        }
    }
}