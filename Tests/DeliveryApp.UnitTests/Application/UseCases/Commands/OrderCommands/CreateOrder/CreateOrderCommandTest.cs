using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.CreateOrder;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Primitives;
using MediatR;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.OrderCommands
{
    public class CreateOrderCommandTest
    {
        [Fact]
        public void CreateOrderCommandShouldBePublic()
        {
            Assert.True(typeof(CreateOrderCommand).IsPublic);
        }

        [Fact]
        public void CreateOrderCommandShouldBeSeald()
        {
            Assert.True(typeof(CreateOrderCommand).IsSealed);
        }

        [Fact]
        public void CreateOrderCommandShouldBeClass()
        {
            Assert.True(typeof(CreateOrderCommand).IsClass);
        }

        [Fact]
        public void CreateOrderCommandShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequest<UnitResult<Error>>), typeof(CreateOrderCommand).GetInterfaces());
        }

        [Theory]
        [InlineData("")]
        [InlineData("              ")]
        [InlineData("   ")]
        [InlineData(null)]
        public void WhenCreatingCreateOrderCommand_AndStreetIsNullOrWhiteSpace_ThenMethodShouldBeThrowArgumentNullException(string street)
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var orderVolume = 4;

            //Act
            Action action = () =>
            {
                new CreateOrderCommand(street, orderId, orderVolume);
            };

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void WhenCreatingCreateOrderCommand_AndOrderIdIsEmpty_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var street = "SomeStreet";
            var orderId = Guid.Empty;
            var orderVolume = 4;

            //Act
            Action action = () =>
            {
                new CreateOrderCommand(street, orderId, orderVolume);
            };

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(-1244125)]
        [InlineData(-5)]
        [InlineData(-99999999)]
        [InlineData(-3333)]
        [InlineData(-9283)]
        [InlineData(-44)]
        [InlineData(-2)]
        public void WhenCreatingCreateOrderCommand_AndOrderVolumeLessOrEqual0_ThenMethodShouldBeThrowArgumentNullException(int orderVolume)
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var street = "SomeStreet";

            //Act
            Action action = () =>
            {
                new CreateOrderCommand(street, orderId, orderVolume);
            };

            //Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Theory]
        [InlineData("Street 1", 1)]
        [InlineData("Street 2", 2)]
        [InlineData("Street 3", 32)]
        [InlineData("Street 4", 1999999)]
        [InlineData("Street 5", 553)]
        [InlineData("Street 6", 12)]
        [InlineData("Street 7", 876)]
        [InlineData("Street 8", int.MaxValue)]
        [InlineData("Street 9", 3)]
        [InlineData("Street 10", 666)]
        public void WhenCreatingCreateOrderCommand_AndInputInfoIsCorrect_ThenMethodShouldBeAddDataToProperties(string street, int orderVolume)
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var command = new CreateOrderCommand(street, orderId, orderVolume);

            //Act-Assert
            command.Should().NotBeNull();
            command.Street.Should().Be(street);
            command.OrderId.Should().Be(orderId);
            command.Volume.Should().Be(orderVolume);
        }
    }
}