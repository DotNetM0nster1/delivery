using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders
{
    public sealed class GetAllNotComplitedOrdersRequest : IRequest<List<OrderDto>>
    {
        public GetAllNotComplitedOrdersRequest(List<OrderDto> orders) 
        {
            if (orders == null)
            { 
                throw new ArgumentNullException(nameof(orders));
            }

            Orders.AddRange(orders);
        }

        public List<OrderDto> Orders { get; private set; } = [];
    }
}