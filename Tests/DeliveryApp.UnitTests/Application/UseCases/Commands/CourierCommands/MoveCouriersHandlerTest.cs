using DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using Primitives;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.CourierCommands
{
    public class MoveCouriersHandlerTest
    {
        private readonly ICourierRepository _courierRepositoryMock = Substitute.For<ICourierRepository>();
        private readonly IOrderRepository _orderRepositoryMock = Substitute.For<IOrderRepository>();
        private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        private readonly MoveCouriersHandler _moveCouriersHandler;

        public MoveCouriersHandlerTest() 
        {
            _moveCouriersHandler = new MoveCouriersHandler(_courierRepositoryMock, _orderRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task WhenHandling_AndAssignedOrdersCount0_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var moveCourierCommand = new MoveCouriersCommand();
            _orderRepositoryMock.GetAllAssignedAsync().Returns([]);

            //Act
            Func<Task> func = async () =>
            {
                await _moveCouriersHandler.Handle(moveCourierCommand, CancellationToken.None); ;
            };

            //Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenHandling_AndNotExistCourierFromThisOrder_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var moveCourierCommand = new MoveCouriersCommand();
            _courierRepositoryMock.GetByIdAsync(Guid.NewGuid()).Returns(Maybe<Courier>.None);

            //Act
            Func<Task> func = async () =>
            {
                await _moveCouriersHandler.Handle(moveCourierCommand, CancellationToken.None); ;
            };

            //Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
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
            _orderRepositoryMock.GetAllAssignedAsync().Returns([order]);
            _courierRepositoryMock.GetByIdAsync(courier.Id).Returns(courier);

            //Act
            await _moveCouriersHandler.Handle(moveCourierCommand, CancellationToken.None);

            //Assert
            courier.StoragePlaces[0].OrderId.Should().Be(null);
            order.Status.Should().Be(OrderStatus.Completed);
        }
    }
}