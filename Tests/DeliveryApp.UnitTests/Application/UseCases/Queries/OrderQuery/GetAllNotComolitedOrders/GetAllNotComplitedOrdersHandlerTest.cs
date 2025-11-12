using DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using DeliveryApp.Core.Ports;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using System.Linq;
using NSubstitute;
using System;
using Xunit;

namespace DeliveryApp.UnitTests.Application.UseCases.Queries.OrderQuery.GetAllNotComolitedOrders
{
    public class GetAllNotComplitedOrdersHandlerTest
    {
        private readonly IAllActiveOrdersResult _allActiveOrdersResultMock = Substitute.For<IAllActiveOrdersResult>();
        private readonly GetAllNotComplitedOrdersHandler _getAllNotComplitedOrdersRequest;

        public GetAllNotComplitedOrdersHandlerTest() 
        {
            _getAllNotComplitedOrdersRequest = new GetAllNotComplitedOrdersHandler(_allActiveOrdersResultMock);
        }

        [Fact]
        public async Task WhenHandling_AndNotExistActiveOrders_ThenMethodShouldThrowArgumentNullException()
        {
            //Arranger
            var x = 6;
            var y = 7;
            var orderId = Guid.NewGuid();
            GetAllNotComplitedOrdersRequest request = null;

            _allActiveOrdersResultMock.GetAllActiveAsync().Returns(request);

            //Act
            Func<Task> func = async () => 
            { 
                await _getAllNotComplitedOrdersRequest.Handle(request, CancellationToken.None); 
            };

            //Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenHandling_AndExistActiveOrders_ThenMethodShouldThrowArgumentNullException()
        {
            //Arranger
            var x = 4;
            var y = 8;
            var orderId = Guid.NewGuid();
            var request = new GetAllNotComplitedOrdersRequest
            ([
                new OrderDto { Id = orderId, Location = new LocationDto { X = x, Y = y} }
            ]);

            _allActiveOrdersResultMock.GetAllActiveAsync().Returns(request);

            //Act
            var handleResult = await _getAllNotComplitedOrdersRequest.Handle(request, CancellationToken.None);

            //Assert
            handleResult.Should().NotBeNull();
            handleResult.Count.Should().Be(1);
            handleResult.First().Id.Should().Be(orderId);
            handleResult.First().Location.X.Should().Be(x);
            handleResult.First().Location.Y.Should().Be(y);
        }
    }
}