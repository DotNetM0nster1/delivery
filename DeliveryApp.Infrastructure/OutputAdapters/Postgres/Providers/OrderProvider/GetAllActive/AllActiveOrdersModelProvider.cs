using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.Extensions.Options;
using DeliveryApp.Core.Ports;
using Npgsql;
using Dapper;
using DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Providers.OrderProvider.GetAllActive
{
    public sealed class AllActiveOrdersModelProvider(IOptions<Settings> options) : IAllActiveOrdersResult
    {
        private readonly IOptions<Settings> _options = options;

        public async Task<GetAllNotComplitedOrdersRequest> GetAllActiveAsync()
        {
            using var connection = new NpgsqlConnection(_options.Value.ConnectionString);
            connection.Open();

            var sqlRequest =
            $"""
            SELECT o.id,
                   o.location_x AS {nameof(LocationDto.X)},
                   o.location_y AS {nameof(LocationDto.Y)}
            FROM orders o
            WHERE status != '{nameof(OrderStatus.Completed).ToLowerInvariant()}'                       
            """;

            var allActiveOrders = await connection.QueryAsync<OrderDto, LocationDto, OrderDto>(
            sqlRequest, (shortOrderDto, locationDto) =>
            {
                shortOrderDto.Location = locationDto;
                return shortOrderDto;
            },
            splitOn: nameof(LocationDto.X));

            return new GetAllNotComplitedOrdersRequest(allActiveOrders.ToList());
        }
    }
}