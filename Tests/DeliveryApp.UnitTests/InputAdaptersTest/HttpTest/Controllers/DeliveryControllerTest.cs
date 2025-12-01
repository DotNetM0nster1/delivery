using DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders;
using DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers;
using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.CreateOrder;
using DeliveryApp.Api.InputAdapters.Http.Contract.Controllers;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Primitives;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.InputAdaptersTest.HttpTest.Controllers
{
    public class DeliveryControllerTest
    {
        private readonly IMediator _mediatorMock = Substitute.For<IMediator>();
        private readonly DeliveryController _deliveryControllerMock;

        public DeliveryControllerTest() 
        {
            _deliveryControllerMock = new DeliveryController(_mediatorMock);
        }

        [Fact]
        public async Task WhenCreatingOrderWithCommand_AndInputDataIsCorrect_ThenMediatorShouldBeCalledAndMethodShouldBeReturnCommandResultOk()
        { 
            //Arrange
             _mediatorMock
                .Send(Arg.Any<CreateOrderCommand>())
                .Returns(UnitResult.Success<Error>());

            //Act
            var commandResult = await _deliveryControllerMock.CreateOrder();

            //Assert
            await _mediatorMock.Received(1).Send(Arg.Any<CreateOrderCommand>());
            commandResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task WhenGettingCouriersQuery_AndCouriersNotExist_ThenMediatorShouldBeCalledAndMethodShouldBeReturnResultNotFound()
        {
            //Arrange 
            _mediatorMock
                .Send(Arg.Any<GetAllBusyCouriersQuery>())
                .Returns((GetAllBusyCouriersResponse)null);

            //Act
            var queryResult = await _deliveryControllerMock.GetCouriers();

            //Assert
            await _mediatorMock.Received(1).Send(Arg.Any<GetAllBusyCouriersQuery>());
            queryResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task WhenGettingCouriersQuery_AndCouriersExist_ThenMediatorShouldBeCalledAndMethodShouldBeReturnResultOk()
        {
            //Arrange 
            _mediatorMock
                .Send(Arg.Any<GetAllBusyCouriersQuery>())
                .Returns(new GetAllBusyCouriersResponse([]));

            //Act
            var queryResult = await _deliveryControllerMock.GetCouriers();

            //Assert
            await _mediatorMock.Received(1).Send(Arg.Any<GetAllBusyCouriersQuery>());
            queryResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task WhenGettingOrdersQuery_AndOrdersNotExist_ThenMediatorShouldBeCalledAndMethodShouldBeReturnResultNotFound()
        {
            //Arrange 
            _mediatorMock
                .Send(Arg.Any<GetAllNotComplitedOrdersQuery>())
                .Returns((GetAllNotComplitedOrdersResponse)null);

            //Act
            var queryResult = await _deliveryControllerMock.GetOrders();

            //Assert
            await _mediatorMock.Received(1).Send(Arg.Any<GetAllNotComplitedOrdersQuery>());
            queryResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task WhenGettingOrdersQuery_AndOrdersNotNull_ThenMediatorShouldBeCalledAndMethodShouldBeReturnResultNotFound()
        {
            //Arrange 
            _mediatorMock
                .Send(Arg.Any<GetAllNotComplitedOrdersQuery>())
                .Returns(new GetAllNotComplitedOrdersResponse([]));

            //Act
            var queryResult = await _deliveryControllerMock.GetOrders();

            //Assert
            await _mediatorMock.Received(1).Send(Arg.Any<GetAllNotComplitedOrdersQuery>());
            queryResult.Should().BeOfType<OkObjectResult>();
        }
    }
}