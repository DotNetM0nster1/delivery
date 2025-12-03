using DeliveryApp.Infrastructure.OutputAdapters.Grpc.GeoService;
using DeliveryApp.Core.Ports;
using Clients.Geo;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Grpc.GeoService
{
    public class GeoClientConfigurationTest
    {
        [Fact]
        public void GeoClientShouldBePublic()
        {
            Assert.True(typeof(GeoClient).IsPublic);
        }

        [Fact]
        public void GeoClientShouldBeSeald()
        {
            Assert.True(typeof(GeoClient).IsSealed);
        }

        [Fact]
        public void GeoClientShouldBeClass()
        {
            Assert.True(typeof(GeoClient).IsClass);
        }

        [Fact]
        public void GeoClientShouldBeSubClassOfDefaultApiController()
        {
            Assert.Contains(typeof(IGeoClient), typeof(GeoClient).GetInterfaces());
        }

        [Fact]
        public void GeoClientBaseConstructor_ShouldBeContains_GeoGeoClient()
        {
            var type = typeof(GeoClient);

            var constructors = type.GetConstructors();

            var constructor = constructors
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    return parameters.Length == 1 &&
                           parameters[0].ParameterType == typeof(Geo.GeoClient);
                });

            Assert.NotNull(constructor);
        }
    }
}