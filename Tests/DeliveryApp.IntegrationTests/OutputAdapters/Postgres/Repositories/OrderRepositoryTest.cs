using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;
using FluentAssertions;
using Testcontainers.PostgreSql;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Repositories
{
    public class OrderRepositoryTest : RepositoryBase<OrderRepository>, IAsyncLifetime 
    {
        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:14.7")
            .WithDatabase("database")
            .WithName("order_repos")
            .WithPassword("password")
            .WithCleanUp(true)
            .Build();

        public async Task InitializeAsync() => await InitializeAsync(_postgreSqlContainer);

        public async Task DisposeAsync() => await DisposeAsync(_postgreSqlContainer);

        [Fact]
        public async Task WhenAddingOrder_AndOrderIsNull_WhenMethodSholdBeThrowArgumentNullException()
        {
            //Arrange
            Func<Task> func = async () =>
            {
                await Repository.AddAsync(null);
            };
            
            //Act-Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenAddingOrder_AndOrderIsCorrect_WhenMethodSholdBeAddOrder()
        {
            //Arrange
            var order = Order.Create(Guid.NewGuid(), Location.Create(3, 9).Value, 20).Value;

            //Act
            await Repository.AddAsync(order);
            await UnitOfWork.SaveChangesAsync();

            //Assert
            var getOrderByIdResult = await Repository.GetByIdAsync(order.Id);
            getOrderByIdResult.Value.Should().BeSameAs(order);
        }

        [Fact]
        public async Task WhenGettingAllAssignedOrders_AndAssignedOrdersNotExist_WhenMethodShouldBeReturnZeroAssignedOrders()
        {
            //Arrange
            var order = Order.Create(Guid.NewGuid(), Location.Create(1, 1).Value, 5).Value;

            //Act-Assert
            var getAllAssignedOrdersResult = await Repository.GetAllAssignedAsync();
            getAllAssignedOrdersResult.Count.Should().Be(0);
        }

        [Fact]
        public async Task WhenGettingAllAssignedOrders_AndSomeOfAssignedOrdersExist_WhenMethodShouldBeReturnSomeOfAssignedOrders()
        {
            //Arrange
            var courier = Courier.Create(1, "Maxim Zakotsky", Location.Create(3, 8).Value).Value;
            var order = Order.Create(Guid.NewGuid(), Location.Create(5, 7).Value, 5).Value;

            order.Assign(courier);

            await Repository.AddAsync(order);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var getAllAssignedOrdersResult = await Repository.GetAllAssignedAsync();

            //Assert
            var assignedOrder = getAllAssignedOrdersResult.First();
            assignedOrder.CourierId.Should().Be(courier.Id);
            assignedOrder.Should().BeSameAs(order);
        }

        [Fact]
        public async Task WhenGettingOrderByGuidId_AndGuidIdIsEmpty_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            Func<Task> func = async () => 
            {
                await Repository.GetByIdAsync(Guid.Empty);
            };

            //Act-Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenGettingOrderByGuidId_AndGuidIdIsCorrect_AndOrderIsNotFound_ThenMethodShouldBeReturnNoneOrder()
        {
            //Arrange-Act
            var fakeOrderId = Guid.NewGuid();
            var orderInDatabase = await Repository.GetByIdAsync(fakeOrderId);

            //Assert
            orderInDatabase.HasNoValue.Should().BeTrue();
        }

        [Fact]
        public async Task WhenGettingOrderByGuidId_AndGuidIdIsCorrect_AndOrderIsFound_ThenMethodShouldBeReturnOrder()
        {
            //Arrange
            var order = Order.Create(Guid.NewGuid(), Location.Create(6, 4).Value, 9).Value;

            await Repository.AddAsync(order);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var getOrderResult = await Repository.GetByIdAsync(order.Id);

            //Assert
            getOrderResult.Value.Should().BeSameAs(order);
        }

        [Fact]
        public async Task WhenGettingFirstCreatedOrder_AndExistOnlyAssignedAndComplitedOrders_ThenMethodShouldBeReturnNoneOrder()
        {
            //Arrange
            var firstOrder = Order.Create(Guid.NewGuid(), Location.Create(5, 7).Value, 5).Value;
            var secondOrder = Order.Create(Guid.NewGuid(), Location.Create(9, 1).Value, 7).Value;
            var firstCourier = Courier.Create(4, "Denis Denisovich", Location.Create(3, 8).Value).Value;
            var secondCourier = Courier.Create(6, "Denis Denisovich", Location.Create(3, 8).Value).Value;

            firstOrder.Assign(firstCourier);
            secondOrder.Assign(secondCourier);
            secondOrder.Complete();

            await Repository.AddAsync(firstOrder);
            await Repository.AddAsync(secondOrder);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var getFirstOrderWithCreatedStatusResult = await Repository.GetFirstWithCreatedStatusAsync();

            //Assert
            getFirstOrderWithCreatedStatusResult.HasNoValue.Should().BeTrue();
        }

        [Fact]
        public async Task WhenGettingFirstCreatedOrder_AndOrderWithCreatedStatusIsExist_ThenMethodShouldBeReturnOrderWithCreatedStatus()
        {
            //Arrange
            var order = Order.Create(Guid.NewGuid(), Location.Create(1, 2).Value, 3).Value;
            var courier = Courier.Create(4, "Valery Orehov", Location.Create(2, 3).Value).Value;

            await Repository.AddAsync(order);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var getFirstOrderWithCreatedStatusResult = await Repository.GetFirstWithCreatedStatusAsync();

            //Assert
            getFirstOrderWithCreatedStatusResult.Value.Should().BeSameAs(order);
        }

        [Fact]
        public void WhenUpdatingOrder_AndOrderIsNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            Action action = () => 
            {
                Repository.Update(null);
            };

            //Act-Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenUpdatingOrder_AndOrderIsHaveChanges_AndOrderStatusWillBeChanged_ThenMethodShouldBeUpdateOrder()
        {
            //Arrange
            var order = Order.Create(Guid.NewGuid(), Location.Create(2, 3).Value, 10).Value;
            var courier = Courier.Create(1, "Slow Man", Location.Create(9, 9).Value).Value;

            await ExecuteInNewDatabaseContextAsync(async (repository, unitOfWork) =>
            {
                await repository.AddAsync(order);
                await unitOfWork.SaveChangesAsync();
            }, _postgreSqlContainer);

            //Act
            await ExecuteInNewDatabaseContextAsync(async (repository, unitOfWork) =>
            {
                order.Assign(courier);
                repository.Update(order);
                await unitOfWork.SaveChangesAsync();
            }, _postgreSqlContainer);

            //Assert
            var getOrderByIdResult = await Repository.GetByIdAsync(order.Id);
            getOrderByIdResult.Value.Status.Name.Should().Be("assigned");
        }
    }
}