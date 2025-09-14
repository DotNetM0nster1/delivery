using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainTest.Model.OrderAggregate.OrderTest
{
    public class CreateTest
    {
        [Fact]
        public void WhenCreatingOrder_AndValuesIsCorrect_ThenMethodShouldBeCreateOrder()
        {
            //Arrange
            var x = 5;
            var y = 6;
            var orderLocation = Location.Create(x, y).Value;

            var orderId = Guid.NewGuid();
            var orderVolume = 2;

            //Act
            var order = Order.Create(orderId, orderLocation, orderVolume);

            //Assert
            Assert.False(order.IsFailure);
            Assert.True(order.IsSuccess);
            Assert.True(order.Value.Id == orderId);
            Assert.True(order.Value.Location == orderLocation);
        }

        [Fact]
        public void WhenCreatingOrder_AndOrderIdIsNotCorrect_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var x = 9;
            var y = 1;
            var orderLocation = Location.Create(x, y).Value;

            var orderId = Guid.Empty;
            var orderVolume = 10;

            //Act
            var order = Order.Create(orderId, orderLocation, orderVolume);

            //Assert
            Assert.True(order.IsFailure);
            Assert.False(order.IsSuccess);
            Assert.True(order.Error.Code == "value.is.required");
            Assert.Throws<ResultFailureException<Error>>(() => order.Value);
        }

        [Fact]
        public void WhenCreatingOrder_AndOrderLocationIsNull_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var orderVolume = 91;
            var orderId = Guid.NewGuid();
            Location orderLocation = null;

            //Act
            var order = Order.Create(orderId, orderLocation, orderVolume);

            //Assert
            Assert.True(order.IsFailure);
            Assert.False(order.IsSuccess);
            Assert.True(order.Error.Code == "value.is.required");
            Assert.Throws<ResultFailureException<Error>>(() => order.Value);
        }

        [Fact]
        public void WhenCreatingOrder_AndOrderVolumeLessOrEqual0_ThenMethodShouldBeReturnError()
        {
            //Arrange
            var x = 1;
            var y = 4;
            var orderLocation = Location.Create(x, y).Value;

            var orderId = Guid.NewGuid();
            var orderVolume = -3;

            //Act
            var order = Order.Create(orderId, orderLocation, orderVolume);

            //Assert
            Assert.True(order.IsFailure);
            Assert.False(order.IsSuccess);
            Assert.True(order.Error.Code == "value.is.invalid");
            Assert.Throws<ResultFailureException<Error>>(() => order.Value);
        }
    }
}