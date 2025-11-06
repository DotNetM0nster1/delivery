using DeliveryApp.Core.Application.UseCases.Queries.Order.GetAllNotComplitedOrders;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Ports;
using Npgsql;
using Dapper;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Queries.Order.GetAllActive
{
    public sealed class AllActiveOrdersResult(string connectionString) : IAllActiveOrdersResult
    {
        private readonly string _connectionString = connectionString;

        public async Task<GetAllNotComplitedOrdersRequest> GetAllActiveAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var sqlRequest =
            @"
                SELECT id, courier_id, location_x, location_y
                FROM public.orders 
                WHERE status != @status;
            ";

            if (await connection.QueryAsync<OrderDto>(sqlRequest, new { status = OrderStatus.Completed.Name }) is
                var allActiveOrders && (allActiveOrders == null || allActiveOrders.Count() == 0))
            {
                throw new ArgumentNullException(nameof(allActiveOrders));
            }

            return new GetAllNotComplitedOrdersRequest(allActiveOrders.ToList());
        }
    }
}