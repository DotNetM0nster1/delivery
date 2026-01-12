using CSharpFunctionalExtensions;
using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.ChangeOrder;
using DeliveryApp.Core.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using Primitives;
using System.Linq;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.OrderCommands
{
    public class CreateOrderHandlerConfigurationTest
    {
        [Fact]
        public void CreateOrderHandlerShouldBePublic()
        {
            Assert.True(typeof(CreateOrderHandler).IsPublic);
        }

        [Fact]
        public void CreateOrderHandlerShouldBeSeald()
        {
            Assert.True(typeof(CreateOrderHandler).IsSealed);
        }

        [Fact]
        public void CreateOrderHandlerShouldBeClass()
        {
            Assert.True(typeof(CreateOrderHandler).IsClass);
        }

        [Fact]
        public void CreateOrderHandlerShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequestHandler<CreateOrderCommand, UnitResult<Error>>), typeof(CreateOrderHandler).GetInterfaces());
        }

        [Fact]
        public void CreateOrderHandlerBaseConstructor_ShouldBeContains_IOrderRepository_AndICourierRepository_AndIUnitOfWork()
        {
            var type = typeof(CreateOrderHandler);

            var constructors = type.GetConstructors();

            var constructor = constructors
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    return parameters.Length == 3 &&
                           parameters[0].ParameterType == typeof(ILogger<CreateOrderHandler>) &&
                           parameters[1].ParameterType == typeof(IOrderRepository) &&
                           parameters[2].ParameterType == typeof(IUnitOfWork);
                });

            Assert.NotNull(constructor);
        }
    }
}