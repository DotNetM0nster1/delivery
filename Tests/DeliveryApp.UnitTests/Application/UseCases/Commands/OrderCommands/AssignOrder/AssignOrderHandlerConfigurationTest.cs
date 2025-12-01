using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.AssignOrder;
using DeliveryApp.Core.Domain.Services.Distribute;
using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using System.Linq;
using Primitives;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.OrderCommands
{
    public class AssignOrderHandlerConfigurationTest
    {
        [Fact]
        public void AssignOrderHandlerShouldBePublic()
        {
            Assert.True(typeof(AssignOrdersHandler).IsPublic);
        }

        [Fact]
        public void AssignOrderHandlerShouldBeSeald()
        {
            Assert.True(typeof(AssignOrdersHandler).IsSealed);
        }

        [Fact]
        public void AssignOrderHandlerShouldBeClass()
        {
            Assert.True(typeof(AssignOrdersHandler).IsClass);
        }

        [Fact]
        public void AssignOrderHandlerShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequestHandler<AssignOrdersCommand, UnitResult<Error>>), typeof(AssignOrdersHandler).GetInterfaces());
        }

        [Fact]
        public void AssignOrderHandlerBaseConstructor_ShouldBeContains_ICourierDistributorService_AndICourierRepository_AndIOrderRepository_AndIUnitOfWork()
        {
            var type = typeof(AssignOrdersHandler);

            var constructors = type.GetConstructors();

            var constructor = constructors
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();

                    return parameters.Length == 5 &&
                           parameters[0].ParameterType == typeof(ICourierDistributorService) &&
                           parameters[1].ParameterType == typeof(ICourierRepository) &&
                           parameters[2].ParameterType == typeof(ILogger<AssignOrdersHandler>) &&
                           parameters[3].ParameterType == typeof(IOrderRepository) &&
                           parameters[4].ParameterType == typeof(IUnitOfWork);
                });

            Assert.NotNull(constructor);
        }
    }
}