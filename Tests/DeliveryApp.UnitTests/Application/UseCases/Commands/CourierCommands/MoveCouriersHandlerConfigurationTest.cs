using DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using System.Linq;
using Primitives;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.CourierCommands
{
    public class MoveCouriersHandlerConfigurationTest
    {
        [Fact]
        public void MoveCouriersHandler_ShouldBePublic()
        {
            Assert.True(typeof(MoveCouriersHandler).IsPublic);
        }

        [Fact]
        public void MoveCouriersHandler_ShouldBeSeald()
        {
            Assert.True(typeof(MoveCouriersHandler).IsSealed);
        }

        [Fact]
        public void MoveCouriersHandler_ShouldBeClass()
        {
            Assert.True(typeof(MoveCouriersHandler).IsClass);
        }

        [Fact]
        public void MoveCouriersHandlerShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequestHandler<MoveCouriersCommand, UnitResult<Error>>), typeof(MoveCouriersHandler).GetInterfaces());
        }

        [Fact]
        public void MoveCouriersHandlerBaseConstructor_ShouldBeContains_ICourierRepository_AndIOrderRepository_AndIUnitOfWork()
        {
            var type = typeof(MoveCouriersHandler);

            var constructors = type.GetConstructors();

            var constructor = constructors
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    return parameters.Length == 4 &&
                           parameters[0].ParameterType == typeof(ICourierRepository) &&
                           parameters[1].ParameterType == typeof(ILogger<MoveCouriersHandler>) &&
                           parameters[2].ParameterType == typeof(IOrderRepository) &&
                           parameters[3].ParameterType == typeof(IUnitOfWork);
                });

            Assert.NotNull(constructor);
        }
    }
}