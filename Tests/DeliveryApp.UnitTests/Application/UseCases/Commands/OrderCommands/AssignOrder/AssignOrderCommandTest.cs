using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.AssignOrder;
using CSharpFunctionalExtensions;
using Primitives;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.OrderCommands
{
    public class AssignOrderCommandTest
    {
        [Fact]
        public void AssignOrderCommandShouldBePublic()
        {
            Assert.True(typeof(AssignOrdersCommand).IsPublic);
        }

        [Fact]
        public void AssignOrderCommandShouldBeSeald()
        {
            Assert.True(typeof(AssignOrdersCommand).IsSealed);
        }

        [Fact]
        public void AssignOrderCommandShouldBeClass()
        {
            Assert.True(typeof(AssignOrdersCommand).IsClass);
        }

        [Fact]
        public void AssignOrderCommandShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequest<UnitResult<Error>>), typeof(AssignOrdersCommand).GetInterfaces());
        }
    }
}