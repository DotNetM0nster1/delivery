using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;
using DotNet.Testcontainers.Builders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Repositories
{
    public class OrderRepositoryTest : IAsyncLifetime
    {
        private ApplicationDatabaseContext _databaseContext;

        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:14.7")
            .WithDatabase("order")
            .WithName("ordername")
            .WithPassword("orderpassword")
            .WithCleanUp(true)
            .Build();

        public async Task DisposeAsync() => await _postgreSqlContainer.DisposeAsync().AsTask();

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();

            var databaseContext = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
                .UseNpgsql(_postgreSqlContainer.GetConnectionString(), options => 
                    { 
                        options.MigrationsAssembly("DeliveryApp.Infrastructure"); 
                    })
                .Options;

            _databaseContext = new ApplicationDatabaseContext(databaseContext);
            _databaseContext.Database.Migrate();
        }

        [Fact]
        public async Task WhenAddingOrder_AndOrderIsNull_WhenMethodSholdBeThrowNullArgumentException()
        {
            //Arrange
            Order order = null;
            var orderReposetory = new OrderRepository(_databaseContext);

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await orderReposetory.AddAsync(order));
        }

        [Fact]
        public async Task WhenAddingOrder_AndOrderIsNotNullAndCorrect_WhenMethodSholdBeAddOrder()
        {
            //Arrange
            var orderTotalVolume = 20;
            var orderId = Guid.NewGuid();
            var orderLocation = Location.Create(3, 9).Value;
            var order = Order.Create(orderId, orderLocation, orderTotalVolume).Value;

            var orderReposetory = new OrderRepository(_databaseContext);

            var unitOfWork = new UnitOfWork(_databaseContext);

            //Act
            await orderReposetory.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(await orderReposetory.GetAsync(order.Id), order);
        }

        [Fact]
        public async Task WhenGettingAllAssignedOrders_AndAssignedOrdersNotExist_WhenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var orderTotalVolume = 5;
            var orderId = Guid.NewGuid();
            var orderLocation = Location.Create(1, 1).Value;
            var order = Order.Create(orderId, orderLocation, orderTotalVolume).Value;

            var orderReposetory = new OrderRepository(_databaseContext);

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>  await orderReposetory.GetAllAssignedAsync());
        }

        [Fact]
        public async Task WhenGettingAllAssignedOrders_AndSomeOfAssignedOrdersExist_WhenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var orderX = 5;
            var orderY = 7;
            var orderTotalVolume = 5;
            var orderId = Guid.NewGuid();
            var orderLocation = Location.Create(orderX, orderY).Value;
            var order = Order.Create(orderId, orderLocation, orderTotalVolume).Value;

            var courierX = 3;
            var courierY = 8;
            var location = Location.Create(courierX, courierY).Value;

            var courierSpeed = 1;
            var courierName = "Maxim Zakotsky";
            var courier = Courier.Create(courierSpeed, courierName, location).Value;

            order.Assign(courier);

            await orderReposetory.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            //Act
            var allAssignedOrdersResult = await orderReposetory.GetAllAssignedAsync();

            //Assert
            var assignedOrder = allAssignedOrdersResult.First();

            Assert.True(assignedOrder.Id == order.Id);
            Assert.True(allAssignedOrdersResult.Count == 1);
            Assert.True(assignedOrder.Location.X == orderX);
            Assert.True(assignedOrder.Location.Y == orderY);
            Assert.True(assignedOrder.Volume == order.Volume);
            Assert.True(assignedOrder.CourierId == courier.Id);
            Assert.True(assignedOrder.Status == OrderStatus.Assigned);
        }

        [Fact]
        public async Task WhenGettingOrderByGuidId_AndGuidIdIsEmpty_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var orderId = Guid.Empty;

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await orderReposetory.GetAsync(orderId));
        }

        [Fact]
        public async Task WhenGettingOrderByGuidId_AndGuidIdIsCorrect_AndOrderIsNotFound_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var orderId = Guid.NewGuid();

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await orderReposetory.GetAsync(orderId));
        }

        [Fact]
        public async Task WhenGettingOrderByGuidId_AndGuidIdIsCorrect_AndOrderIsFound_ThenMethodShouldBeReturnOrder()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var orderX = 6;
            var orderY = 4;
            var orderTotalVolume = 9;
            var orderId = Guid.NewGuid();
            var orderLocation = Location.Create(orderX, orderY).Value;
            var order = Order.Create(orderId, orderLocation, orderTotalVolume).Value;

            await orderReposetory.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            //Act
            var getOrderResult = await orderReposetory.GetAsync(orderId);

            //Assert
            Assert.Equal(getOrderResult.Id, order.Id);
            Assert.Equal(getOrderResult.Status, order.Status);
            Assert.Equal(getOrderResult.Volume, order.Volume);
            Assert.Equal(getOrderResult.Location.X, order.Location.X);
            Assert.Equal(getOrderResult.Location.Y, order.Location.Y);
        }

        [Fact]
        public async Task WhenGettingFirstCreatedOrder_AndExistOnlyAssignedAndComplitedOrders_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var firstrderX = 5;
            var firstOrderY = 7;
            var firstOrderTotalVolume = 5;
            var firstOrderId = Guid.NewGuid();
            var firstOrderLocation = Location.Create(firstrderX, firstOrderY).Value;
            var firstOrder = Order.Create(firstOrderId, firstOrderLocation, firstOrderTotalVolume).Value;

            var secondrderX = 9;
            var secondOrderY = 1;
            var secondOrderTotalVolume = 7;
            var secondOrderId = Guid.NewGuid();
            var secondOrderLocation = Location.Create(secondrderX, secondOrderY).Value;
            var secondOrder = Order.Create(secondOrderId, secondOrderLocation, secondOrderTotalVolume).Value;

            var firstCourierX = 3;
            var firstCourierY = 8;
            var firstLocation = Location.Create(firstCourierX, firstCourierY).Value;

            var firstCourierSpeed = 4;
            var firstCourierName = "Denis Denisovich";
            var firstCourier = Courier.Create(firstCourierSpeed, firstCourierName, firstLocation).Value;

            var secondCourierX = 3;
            var secondCourierY = 8;
            var secondLocation = Location.Create(secondCourierX, secondCourierY).Value;

            var secondCourierSpeed = 6;
            var secondCourierName = "Denis Denisovich";
            var secondCourier = Courier.Create(secondCourierSpeed, secondCourierName, secondLocation).Value;

            firstOrder.Assign(firstCourier);
            secondOrder.Assign(secondCourier);
            secondOrder.Complete();

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await orderReposetory.GetFirstWithCreatedStatusAsync());
        }

        [Fact]
        public async Task WhenGettingFirstCreatedOrder_AndOrderWithCreatedStatusIsExist_ThenMethodShouldBe()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var orderX = 1;
            var orderY = 2;
            var orderTotalVolume = 3;
            var orderId = Guid.NewGuid();
            var orderLocation = Location.Create(orderX, orderY).Value;
            var order = Core.Domain.Model.OrderAggregate.Order.Create(orderId, orderLocation, orderTotalVolume).Value;

            var courierX = 2;
            var courierY = 3;
            var location = Location.Create(courierX, courierY).Value;

            var courierSpeed = 4;
            var courierName = "Valery Orehov";
            var courier = Courier.Create(courierSpeed, courierName, location).Value;

            await orderReposetory.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            //Act
            var getFirstOrderWithCreatedStatusResult = await orderReposetory.GetFirstWithCreatedStatusAsync();

            //Assert
            Assert.Equal(getFirstOrderWithCreatedStatusResult, order);
        }

        [Fact]
        public void WhenUpdatingOrder_AndOrderIsNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);
            
            //Act
            Order order = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => orderReposetory.Update(order));
        }

        [Fact]
        public async Task WhenUpdatingOrder_AndOrderNotHaveChanges_ThenMethodShouldntBeUpdateOrder()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var orderX = 7;
            var orderY = 4;
            var orderTotalVolume = 8;
            var orderId = Guid.NewGuid();
            var orderLocation = Location.Create(orderX, orderY).Value;
            var order = Order.Create(orderId, orderLocation, orderTotalVolume).Value;

            var courierX = 3;
            var courierY = 5;
            var location = Location.Create(courierX, courierY).Value;

            var courierSpeed = 9;
            var courierName = "Speed Man";
            var courier = Courier.Create(courierSpeed, courierName, location).Value;

            await orderReposetory.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            //Act
            orderReposetory.Update(order);

            await unitOfWork.SaveChangesAsync();

            //Assert
            var orderInDatabase = await orderReposetory.GetAsync(order.Id);
            Assert.Equal(orderInDatabase, order);
        }

        [Fact]
        public async Task WhenUpdatingOrder_AndOrderIsHaveChanges_AndOrderStatusWillBeChanged_ThenMethodShouldBeUpdateOrder()
        {
            //Arrange
            var orderReposetory = new OrderRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var orderX = 2;
            var orderY = 3;
            var orderTotalVolume = 10;
            var orderId = Guid.NewGuid();
            var orderLocation = Location.Create(orderX, orderY).Value;
            var order = Order.Create(orderId, orderLocation, orderTotalVolume).Value;

            var courierX = 9;
            var courierY = 9;
            var location = Location.Create(courierX, courierY).Value;

            var courierSpeed = 1;
            var courierName = "Slow Man";
            var courier = Courier.Create(courierSpeed, courierName, location).Value;

            await orderReposetory.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            //Act
            order.Assign(courier);

            orderReposetory.Update(order);

            await unitOfWork.SaveChangesAsync();

            //Assert
            var orderInDatabase = await orderReposetory.GetAsync(order.Id);
            Assert.Equal("assigned", orderInDatabase.Status.Name);
        }
    }
}