using DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using System.Collections.Generic;
using DeliveryApp.Core.Ports;
using System.Linq;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Queries.OrderQuery.GetAllNotComolitedOrders
{
    public class GetAllNotComplitedOrdersHandlerConfigurationTest
    {
        [Fact]
        public void GetAllNotComplitedOrdersHandlerShouldBePublic()
        {
            Assert.True(typeof(GetAllNotComplitedOrdersHandler).IsPublic);
        }

        [Fact]
        public void GetAllNotComplitedOrdersHandlerShouldBeSeald()
        {
            Assert.True(typeof(GetAllNotComplitedOrdersHandler).IsSealed);
        }

        [Fact]
        public void GetAllNotComplitedOrdersHandlerShouldBeClass()
        {
            Assert.True(typeof(GetAllNotComplitedOrdersHandler).IsClass);
        }

        [Fact]
        public void GetAllNotComplitedOrdersHandlerShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequestHandler<GetAllNotComplitedOrdersRequest, List<OrderDto>>), typeof(GetAllNotComplitedOrdersHandler).GetInterfaces());
        }

        [Fact]
        public void GetAllNotComplitedOrdersHandlerBaseConstructor_ShouldBeContains_IAllActiveOrdersResult()
        {
            var type = typeof(GetAllNotComplitedOrdersHandler);

            var constructors = type.GetConstructors();

            var constructor = constructors
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    return parameters.Length == 1 &&
                           parameters[0].ParameterType == typeof(IAllActiveOrdersResult);
                });

            Assert.NotNull(constructor);
        }
    }
}