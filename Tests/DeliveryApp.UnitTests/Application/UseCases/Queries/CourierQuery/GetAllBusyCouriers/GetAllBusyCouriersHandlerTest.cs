using DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers;
using DeliveryApp.Core.Ports;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using DeliveryApp.Core.Application.UseCases.Queries.Dto;
using System;
using System.Threading;
using FluentAssertions;
using System.Linq;
using System.Net.Http;

namespace DeliveryApp.UnitTests.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers
{
    public class GetAllBusyCouriersHandlerTest
    {
        private readonly IAllBusyCouriersModelProvider _allBusyCouriersModelProviderMock = Substitute.For<IAllBusyCouriersModelProvider>();

        private readonly GetAllBusyCouriersHandler _getAllBusyCouriersHandler;

        public GetAllBusyCouriersHandlerTest() 
        {
            _getAllBusyCouriersHandler = new GetAllBusyCouriersHandler(_allBusyCouriersModelProviderMock);
        }

        [Fact]
        public async Task WhenHandling_AndNotExistBusyCouriers_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var x = 2;
            var y = 9;
            var courierId = Guid.NewGuid();
            GetAllBusyCouriersRequest request = null;

            _allBusyCouriersModelProviderMock.GetAllBusyCouriersAsync().Returns(request);

            //Act
            Func<Task> func = async () => 
            { 
                await _getAllBusyCouriersHandler.Handle(request, CancellationToken.None); 
            };

            //Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenHandling_AndExistBusyCouriers_ThenMethodShouldBeReturnListOfCouriers()
        {
            //Arrange
            var x = 2;
            var y = 9;
            var courierId = Guid.NewGuid();
            var request = new GetAllBusyCouriersRequest
            ([
                new CourierDto { Id = courierId, Location = new LocationDto { X = x, Y = y } }
            ]);

            _allBusyCouriersModelProviderMock.GetAllBusyCouriersAsync().Returns(request);

            //Act
            var handleResult = await _getAllBusyCouriersHandler.Handle(request, CancellationToken.None);

            //Assert
            handleResult.Should().NotBeNull();
            handleResult.Count.Should().Be(1);
            handleResult.First().Id.Should().Be(courierId);
            handleResult.First().Location.X.Should().Be(x);
            handleResult.First().Location.Y.Should().Be(y);
        }
    }
}