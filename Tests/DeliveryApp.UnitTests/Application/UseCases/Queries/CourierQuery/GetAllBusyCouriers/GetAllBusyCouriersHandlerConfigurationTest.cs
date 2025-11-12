using DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using System.Collections.Generic;
using DeliveryApp.Core.Ports;
using System.Linq;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers
{
    public class GetAllBusyCouriersHandlerConfigurationTest
    {
        [Fact]
        public void GetAllBusyCouriersHandlerShouldBePublic()
        {
            Assert.True(typeof(GetAllBusyCouriersHandler).IsPublic);
        }

        [Fact]
        public void GetAllBusyCouriersHandlerShouldBeSeald()
        {
            Assert.True(typeof(GetAllBusyCouriersHandler).IsSealed);
        }

        [Fact]
        public void GetAllBusyCouriersHandlerShouldBeClass()
        {
            Assert.True(typeof(GetAllBusyCouriersHandler).IsClass);
        }

        [Fact]
        public void GetAllBusyCouriersHandlerShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequestHandler<GetAllBusyCouriersRequest, List<CourierDto>>), typeof(GetAllBusyCouriersHandler).GetInterfaces());
        }

        [Fact]
        public void GetAllBusyCouriersHandlerBaseConstructor_ShouldBeContains_IAllBusyCouriersModelProvider()
        {
            var type = typeof(GetAllBusyCouriersHandler);

            var constructors = type.GetConstructors();

            var constructor = constructors
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    return parameters.Length == 1 &&
                           parameters[0].ParameterType == typeof(IAllBusyCouriersModelProvider);
                });

            Assert.NotNull(constructor);
        }
    }
}