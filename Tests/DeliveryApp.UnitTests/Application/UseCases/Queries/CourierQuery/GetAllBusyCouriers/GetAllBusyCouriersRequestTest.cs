using DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers;
using DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using System.Collections.Generic;
using FluentAssertions;
using MediatR;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers
{
    public class GetAllBusyCouriersRequestTest
    {
        [Fact]
        public void GetAllBusyCouriersRequestShouldBePublic()
        {
            Assert.True(typeof(GetAllBusyCouriersRequest).IsPublic);
        }

        [Fact]
        public void GetAllBusyCouriersRequestShouldBeSeald()
        {
            Assert.True(typeof(MoveCouriersCommand).IsSealed);
        }

        [Fact]
        public void GetAllBusyCouriersRequestShouldBeClass()
        {
            Assert.True(typeof(GetAllBusyCouriersRequest).IsClass);
        }

        [Fact]
        public void GetAllBusyCouriersRequestShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequest<List<CourierDto>>), typeof(GetAllBusyCouriersRequest).GetInterfaces());
        }

        [Fact]
        public void WhenCreateGetAllBusyCouriersRequest_AndCouriersNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            Action action = () => 
            {
                new GetAllBusyCouriersRequest(null);
            };

            //Acr-Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void WhenCreatingGetAllBusyCouriersRequest_AndCouriersNotNull_ThenMethodShouldBeAddCouriersIntoList()
        {
            //Arrange
            var getAllBusyCouriersRequest = new GetAllBusyCouriersRequest
            ([
                new CourierDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 7, Y = 8} },
                new CourierDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 9, Y = 3} },
                new CourierDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 2, Y = 2} },
                new CourierDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 4, Y = 5} },
                new CourierDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 3, Y = 9} },
                new CourierDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 7, Y = 4} }
            ]);


            //Act-Assert
            getAllBusyCouriersRequest.Should().NotBeNull();
            getAllBusyCouriersRequest.Couriers.Should().NotBeNull();
            getAllBusyCouriersRequest.Couriers.Count.Should().Be(6);
        }
    }
}