using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.CreateOrder;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using System.Threading.Tasks;
using DeliveryApp.Core.Ports;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using Primitives;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.OrderCommands
{
    public sealed class CreateOrderHandlerTest
    {
        private readonly IOrderRepository _orderRepositoryMock = Substitute.For<IOrderRepository>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly CreateOrderHandler _createOrderHandler;

        public CreateOrderHandlerTest() 
        {
            _createOrderHandler = new CreateOrderHandler(_orderRepositoryMock, _unitOfWork);
        }

        [Fact]
        public async Task WhenHandling_AndInputDataIsCorrect_ThenMethodShouldBeAddOrderInDatabase()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, Location.Create(4, 5).Value, 3).Value;
            var command = new CreateOrderCommand("SomeStreet", orderId, 4);

            //Act
            var handleResul = await _createOrderHandler.Handle(command, CancellationToken.None);

            //Assert
            handleResul.Should().NotBeNull();
            handleResul.IsSuccess.Should().BeTrue();    
            await _orderRepositoryMock.Received(1).AddAsync(order);
        }
    }
}