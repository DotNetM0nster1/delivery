using DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using System.Collections.Generic;
using FluentAssertions;
using MediatR;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Queries.OrderQuery.GetAllNotComolitedOrders
{
    public class GetAllNotComplitedOrdersRequestTest
    {
        [Fact]
        public void GetAllNotComplitedOrdersRequestShouldBePublic()
        {
            Assert.True(typeof(GetAllNotComplitedOrdersRequest).IsPublic);
        }

        [Fact]
        public void GetAllNotComplitedOrdersRequestShouldBeSeald()
        {
            Assert.True(typeof(GetAllNotComplitedOrdersRequest).IsSealed);
        }

        [Fact]
        public void GetAllNotComplitedOrdersRequestShouldBeClass()
        {
            Assert.True(typeof(GetAllNotComplitedOrdersRequest).IsClass);
        }

        [Fact]
        public void GetAllNotComplitedOrdersRequestShouldBeSubClassOfIrequestUnitResultError()
        {
            Assert.Contains(typeof(IRequest<List<OrderDto>>), typeof(GetAllNotComplitedOrdersRequest).GetInterfaces());
        }

        [Fact]
        public void WhenCreatingGetAllNotComplitedOrdersRequest_AndCouriersNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            Action action = () =>
            {
                new GetAllNotComplitedOrdersRequest(null);
            };

            //Acr-Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void WhenCreateGetAllBusyCouriersRequest_AndOrdersNotNull_ThenMethodShouldBeAddCouriersIntoList()
        {
            //Arrange
            var getAllNotComplitedOrdersRequest = new GetAllNotComplitedOrdersRequest
            ([
                new OrderDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 7, Y = 8} },
                new OrderDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 9, Y = 3} },
                new OrderDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 2, Y = 2} },
                new OrderDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 4, Y = 5} },
                new OrderDto { Id = Guid.NewGuid(), Location = new LocationDto { X = 3, Y = 9} },
            ]);

            //Act-Assert
            getAllNotComplitedOrdersRequest.Should().NotBeNull();
            getAllNotComplitedOrdersRequest.Orders.Should().NotBeNull();
            getAllNotComplitedOrdersRequest.Orders.Count.Should().Be(5);
        }
    }
}