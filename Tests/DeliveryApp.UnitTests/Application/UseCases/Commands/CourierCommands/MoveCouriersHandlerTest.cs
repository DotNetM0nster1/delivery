using DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using System.Threading.Tasks;
using FluentAssertions;
using System.Threading;
using NSubstitute;
using Primitives;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.CourierCommands
{
    public class MoveCouriersHandlerTest
    {
        private readonly ILogger<MoveCouriersHandler> _loggerMock = Substitute.For<ILogger<MoveCouriersHandler>>();
        private readonly ICourierRepository _courierRepositoryMock = Substitute.For<ICourierRepository>();
        private readonly IOrderRepository _orderRepositoryMock = Substitute.For<IOrderRepository>();
        private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        private readonly MoveCouriersHandler _moveCouriersHandler;

        public MoveCouriersHandlerTest() 
        {
            _moveCouriersHandler = new MoveCouriersHandler(_courierRepositoryMock, _loggerMock, _orderRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task WhenHandling_AndAssignedOrdersCount0_ThenMethodShouldBeReturnNotFoundError()
        {
            //Arrange
            var moveCourierCommand = new MoveCouriersCommand();
            _orderRepositoryMock.GetAllAssignedOrdersAsync().Returns([]);

            //Act
            var commandResult = await _moveCouriersHandler.Handle(moveCourierCommand, CancellationToken.None);

            //Assert
            commandResult.Should().NotBeNull();
            commandResult.Error.Should().NotBeNull();
            commandResult.IsFailure.Should().BeTrue();  
            commandResult.Should().BeOfType<UnitResult<Error>>();
            commandResult.Error.Code.Should().BeEquivalentTo("record.not.found");
        }

        [Fact]
        public async Task WhenHandling_AndNotExistCourierFromThisOrder_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var moveCourierCommand = new MoveCouriersCommand();
            _courierRepositoryMock.GetByIdAsync(Guid.NewGuid()).Returns(Maybe<Courier>.None);

            //Act
            var commandResult = await _moveCouriersHandler.Handle(moveCourierCommand, CancellationToken.None);

            //Assert
            commandResult.Should().NotBeNull();
            commandResult.Error.Should().NotBeNull();
            commandResult.IsFailure.Should().BeTrue();
            commandResult.Should().BeOfType<UnitResult<Error>>();
            commandResult.Error.Code.Should().BeEquivalentTo("record.not.found");
        }

        [Fact]
        public async Task WhenHandling_AndCourierLocationEqualsOrderLocation_ThenMethodShouldBeCompleteOrderAndFinishOrderForCourier()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, Location.Create(3, 4).Value, 4).Value;

            var courier = Courier.Create(4, "Ivan Kapustov", Location.Create(3, 4).Value).Value;

            order.Assign(courier);

            var moveCourierCommand = new MoveCouriersCommand();
            _orderRepositoryMock.GetAllAssignedOrdersAsync().Returns([order]);
            _courierRepositoryMock.GetByIdAsync(courier.Id).Returns(courier);

            //Act
            await _moveCouriersHandler.Handle(moveCourierCommand, CancellationToken.None);

            //Assert
            courier.StoragePlaces[0].OrderId.Should().Be(null);
            order.Status.Should().Be(OrderStatus.Completed);
        }
    }
}