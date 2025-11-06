using DeliveryApp.Core.Application.UseCases.Queries.Courier.GetAllBusyCouriers;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using DeliveryApp.Core.Ports;
using Dapper;
using Npgsql;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Queries.Courier.GetAllBusy
{
    public sealed class AllBusyCouriersModelProvider(string connectionString) : IAllBusyCouriersModelProvider
    {
        private readonly string _connectionString = connectionString;

        public async Task<GetAllBusyCouriersRequest> GetAllBusyCouriersAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var sqlRequest = 
            @"
                SELECT id, name, location_x, location_y 
                FROM public.couriers
            ";

            if (await connection.QueryAsync<CourierDto>(sqlRequest, new { }) is
                var allBusyCouriers && (allBusyCouriers == null || allBusyCouriers.Count() == 0))
            {
                throw new ArgumentNullException(nameof(allBusyCouriers));       
            }

            return new GetAllBusyCouriersRequest(allBusyCouriers.ToList());
        }
    }
}