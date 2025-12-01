using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.AssignOrder;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Services.Distribute;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryApp.Core.Ports;
using FluentAssertions;
using System.Threading;
using NSubstitute;
using Primitives;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.OrderCommands
{
    public class AssignOrderHandlerTest
    {
        private readonly ICourierDistributorService _courierDistributorServiceMock = Substitute.For<ICourierDistributorService>();
        private readonly ILogger<AssignOrdersHandler> _loggerMock = Substitute.For<ILogger<AssignOrdersHandler>>();
        private readonly ICourierRepository _courierRepositoryMock = Substitute.For<ICourierRepository>();
        private readonly IOrderRepository _orderRepositoryMock = Substitute.For<IOrderRepository>();
        private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        private readonly AssignOrdersHandler _assignOrderHandler;

        private AssignOrdersCommand _command => new AssignOrdersCommand();

        public AssignOrderHandlerTest()
        {
            _assignOrderHandler = new AssignOrdersHandler(_courierDistributorServiceMock, _courierRepositoryMock, _loggerMock, _orderRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task WhenHandling_AndNotExistOrderWithCreatedStatus_ThenMethodSouldBeReturnNotFoundError()
        {
            //Arrange
            _orderRepositoryMock.GetFirstOrderWithCreatedStatusAsync().Returns(Maybe<Order>.None);

            //Act
            var commandResult = await _assignOrderHandler.Handle(_command, CancellationToken.None);

            //Assert
            commandResult.Should().NotBeNull();
            commandResult.Error.Should().NotBeNull();
            commandResult.IsFailure.Should().BeTrue();
            commandResult.Should().BeOfType<UnitResult<Error>>();
            commandResult.Error.Code.Should().BeEquivalentTo("value.not.found");
        }

        [Fact]
        public async Task WhenHandling_AndNotExistFreeCouriers_ThenMethodSouldBeReturnNotFoundError()
        {
            //Arrange
            var orderid = Guid.NewGuid();
            var order = Order.Create(orderid, Location.Create(3, 6).Value, 5).Value;

            _orderRepositoryMock.GetFirstOrderWithCreatedStatusAsync().Returns(order);
            _courierRepositoryMock.GetAllFreeCouriersAsync().Returns([]);

            //Act
            var commandResult = await _assignOrderHandler.Handle(_command, CancellationToken.None);

            //Assert
            commandResult.Should().NotBeNull();
            commandResult.Error.Should().NotBeNull();
            commandResult.IsFailure.Should().BeTrue();
            commandResult.Should().BeOfType<UnitResult<Error>>();
            commandResult.Error.Code.Should().BeEquivalentTo("value.not.found");
        }

        [Fact]
        public async Task WhenHandling_AndDistrinuteOrderResultIsSuccess_ThenMethodSouldBeReturnSuccessResult()
        {
            //Arrange
            List<Courier> couriers = [Courier.Create(3, "Danil Kilov", Location.Create(3, 8).Value).Value];
            var order = Order.Create(Guid.NewGuid(), Location.Create(5, 1).Value, 3).Value;

            _orderRepositoryMock.GetFirstOrderWithCreatedStatusAsync().Returns(order);
            _courierRepositoryMock.GetAllFreeCouriersAsync().Returns(couriers);

            //Act
            var handleResult = await _assignOrderHandler.Handle(_command, CancellationToken.None);

            //Assert
            handleResult.IsSuccess.Should().BeTrue();
            _courierDistributorServiceMock.Received(1).DistributeOrderOnCouriers(Arg.Any<Order>(), Arg.Any<List<Courier>>());
            _courierRepositoryMock.Received(1).Update(Arg.Any<Courier>());
            _orderRepositoryMock.Received(1).Update(Arg.Any<Order>());
            await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
        }
    }
}