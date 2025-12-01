using DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders;
using DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers;
using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.CreateOrder;
using DeliveryApp.Api.Adapters.Http.Contract.src.OpenApi.Controllers;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace DeliveryApp.Api.InputAdapters.Http.Contract.Controllers
{
    public sealed class DeliveryController(IMediator mediator) : DefaultApiController
    {
        private readonly IMediator _mediator = mediator;

        public override Task<IActionResult> CreateCourier([FromBody] CourierDto courier)
        {
            throw new NotImplementedException();
        }

        public override async Task<IActionResult> CreateOrder()
        {
            var createOrderCommand = new CreateOrderCommand("no_existing_street", Guid.NewGuid(), 1);

            var commandResult = await _mediator.Send(createOrderCommand);

            return commandResult.IsSuccess 
                ? Ok(commandResult) 
                : BadRequest();
        }

        public override async Task<IActionResult> GetCouriers()
        {
            var getCouriersQuery = new GetAllBusyCouriersQuery();

            var queryResult = await _mediator.Send(getCouriersQuery);

            if (queryResult?.Couriers is var couriers && couriers == null)
            {
                return NotFound();
            }

            return Ok(couriers.Select(courier => new CourierDto
            {
                Id = courier.Id,
                Location = new LocationDto
                {
                    X = courier.Location.X,
                    Y = courier.Location.Y,
                },
                Name = courier.Name,
            }));
        }

        public override async Task<IActionResult> GetOrders()
        {
            var getOrdersQuery = new GetAllNotComplitedOrdersQuery();

            var queryResult = await _mediator.Send(getOrdersQuery);

            if (queryResult?.Orders is var orders && orders == null)
            {
                return NotFound();
            }

            return Ok(orders.Select(order => new OrderDto
            {
                Id = order.Id,
                Location = new LocationDto
                {
                    X = order.Location.X,
                    Y = order.Location.Y,
                },
            }));
        }
    }
}