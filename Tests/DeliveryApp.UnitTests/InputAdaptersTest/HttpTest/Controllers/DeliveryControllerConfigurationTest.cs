using DeliveryApp.Api.Adapters.Http.Contract.src.OpenApi.Controllers;
using DeliveryApp.Api.InputAdapters.Http.Contract.Controllers;
using System.Linq;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.InputAdaptersTest.HttpTest.Controllers
{
    public class DeliveryControllerConfigurationTest
    {
        [Fact]
        public void DeliveryControllerShouldBePublic()
        {
            Assert.True(typeof(DeliveryController).IsPublic);
        }

        [Fact]
        public void DeliveryControllerShouldBeSeald()
        {
            Assert.True(typeof(DeliveryController).IsSealed);
        }

        [Fact]
        public void DeliveryControllerShouldBeClass()
        {
            Assert.True(typeof(DeliveryController).IsClass);
        }

        [Fact]
        public void DeliveryControllerShouldBeSubClassOfDefaultApiController()
        {
            Assert.True(typeof(DeliveryController).IsSubclassOf(typeof(DefaultApiController)));
        }

        [Fact]
        public void DeliveryControllerBaseConstructor_ShouldBeContains_IMediator()
        {
            var type = typeof(DeliveryController);

            var constructors = type.GetConstructors();

            var constructor = constructors
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    return parameters.Length == 1 &&
                           parameters[0].ParameterType == typeof(IMediator);
                });

            Assert.NotNull(constructor);
        }
    }
}