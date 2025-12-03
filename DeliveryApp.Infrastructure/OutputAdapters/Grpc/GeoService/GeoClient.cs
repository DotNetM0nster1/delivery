using Location = DeliveryApp.Core.Domain.Model.SharedKernel.Location;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Clients.Geo;
using Primitives;

namespace DeliveryApp.Infrastructure.OutputAdapters.Grpc.GeoService
{
    public sealed class GeoClient(Geo.GeoClient geoClient) : IGeoClient
    {
        private readonly Geo.GeoClient _geoService = geoClient;

        public async Task<Result<Location, Error>> GetLocation(string street)
        {
            var reply = await _geoService.GetGeolocationAsync(new GetGeolocationRequest()
            {
                Street = street
            });

            return Location.Create(reply.Location.X, reply.Location.Y);
        }
    }
}