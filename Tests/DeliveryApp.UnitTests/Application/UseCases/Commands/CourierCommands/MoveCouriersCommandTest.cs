using DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers;
using CSharpFunctionalExtensions;
using Primitives;
using MediatR;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Commands.CourierCommands
{
    public class MoveCouriersCommandTest
    {
        [Fact]
        public void MoveCouriersCommandShouldBePublic()
        {
            Assert.True(typeof(MoveCouriersCommand).IsPublic);
        }

        [Fact]
        public void MoveCouriersCommandShouldBeSeald()
        {
            Assert.True(typeof(MoveCouriersCommand).IsSealed);
        }

        [Fact]
        public void MoveCouriersCommandShouldBeClass()
        {
            Assert.True(typeof(MoveCouriersCommand).IsClass);
        }

        [Fact]
        public void MoveCouriersCommandShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequest<UnitResult<Error>>), typeof(MoveCouriersCommand).GetInterfaces());
        }
    }
}