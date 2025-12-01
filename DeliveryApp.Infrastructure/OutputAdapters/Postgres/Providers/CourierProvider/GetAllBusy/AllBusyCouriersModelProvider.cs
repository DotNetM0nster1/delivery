using DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using Microsoft.Extensions.Options;
using DeliveryApp.Core.Ports;
using Dapper;
using Npgsql;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Providers.CourierProvider.GetAllBusy
{
    public sealed class AllBusyCouriersModelProvider(IOptions<Settings> options) : IAllBusyCouriersModelProvider
    {
        private readonly IOptions<Settings> _options = options;

        public async Task<GetAllBusyCouriersResponse> GetAllBusyCouriersAsync()
        {
            using var connection = new NpgsqlConnection(_options.Value.ConnectionString);
            connection.Open();

            var sqlRequest =
            $"""
            SELECT c.id,
                   c.name,
                   c.location_x AS {nameof(LocationDto.X)},
                   c.location_y AS {nameof(LocationDto.Y)}
            FROM couriers c         
            """;

            var allBusyCouriers = await connection.QueryAsync<CourierDto, LocationDto, CourierDto>(
            sqlRequest, (shortCourierDto, locationDto) =>
            {
                shortCourierDto.Location = locationDto;
                return shortCourierDto;
            },
            splitOn: nameof(LocationDto.X));

            return new GetAllBusyCouriersResponse(allBusyCouriers.ToList());
        }
    }
}