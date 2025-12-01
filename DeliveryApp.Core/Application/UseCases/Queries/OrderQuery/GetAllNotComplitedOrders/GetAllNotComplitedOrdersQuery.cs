using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders
{
    public class GetAllNotComplitedOrdersQuery : IRequest<GetAllNotComplitedOrdersResponse> { }
}