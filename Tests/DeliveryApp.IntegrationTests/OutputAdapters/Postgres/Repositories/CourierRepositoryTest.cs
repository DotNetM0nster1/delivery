using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Repositories
{
    public class CourierRepositoryTest : IAsyncLifetime
    {
        private ApplicationDatabaseContext _databaseContext;

        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:14.7")
            .WithDatabase("courier")
            .WithName("couriername")
            .WithPassword("courierpassword")
            .WithCleanUp(true)
            .Build();

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

        public async Task DisposeAsync() => await _postgreSqlContainer.DisposeAsync().AsTask();

        [Fact]
        public async Task WhenAddingCourier_AndCourierIsNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);

            Courier courier = null;

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await courierRepository.AddAsync(courier));
        }

        [Fact]
        public async Task WhenAddingCourier_AndCourierIsNotNull_ThenMethodShouldBeAddCourier()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var courierX = 5;
            var courierY = 9;
            var currentCurierLocation = Location.Create(courierX, courierY).Value;

            var courierSpeed = 3;
            var courierName = "Kirill Mikrov";
            var courier = Courier.Create(courierSpeed, courierName, currentCurierLocation).Value;

            //Act
            await courierRepository.AddAsync(courier);
            await unitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(await courierRepository.GetByIdAsync(courier.Id), courier);
        }

        [Fact]
        public async Task WhenGettingAllFreeCouriers_AndFreeCouriersNotFound_ThenMethodShouldBeReturnZeroCountCouriers()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var courierX = 5;
            var courierY = 9;
            var currentCurierLocation = Location.Create(courierX, courierY).Value;

            var courierSpeed = 3;
            var courierName = "Kirill Mikrov";
            var courier = Courier.Create(courierSpeed, courierName, currentCurierLocation).Value;

            var orderX = 3;
            var orderY = 2;
            var orderLocation = Location.Create(orderX, orderY).Value;

            var orderVolume = 7;
            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId, orderLocation, orderVolume).Value;

            courier.TakeOrder(order);

            await courierRepository.AddAsync(courier);
            await unitOfWork.SaveChangesAsync();

            //Act-Assert
            var getAllCouriuersResult = await courierRepository.GetAllFreeCouriersAsync();
            Assert.True(getAllCouriuersResult.Count == 0);
        }

        [Fact]
        public async Task WhenGettingAllFreeCouriers_AndFreeCouriersFound_ThenMethodShouldBeReturnFreeCouriers()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var firstCourierX = 5;
            var firstCourierY = 9;
            var firstCurrentCurierLocation = Location.Create(firstCourierX, firstCourierY).Value;

            var firstCourierSpeed = 3;
            var firstCourierName = "Kirill Mikrov";
            var firstCourier = Courier.Create(firstCourierSpeed, firstCourierName, firstCurrentCurierLocation).Value;

            var secondCourierX = 2;
            var secondCourierY = 2;
            var secondCurrentCurierLocation = Location.Create(secondCourierX, secondCourierY).Value;

            var secondCourierSpeed = 5;
            var secondCourierName = "Alexandr Ivin";
            var secondCourier = Courier.Create(secondCourierSpeed, secondCourierName, secondCurrentCurierLocation).Value;

            await courierRepository.AddRangeAsync([firstCourier, secondCourier]);
            await unitOfWork.SaveChangesAsync();

            //Act
            var allFreeCouriersResult = await courierRepository.GetAllFreeCouriersAsync();

            //Assert
            Assert.True(allFreeCouriersResult.Count == 2);
            Assert.Contains(firstCourier, allFreeCouriersResult);
            Assert.Contains(secondCourier, allFreeCouriersResult);
        }

        [Fact]
        public async Task WhenAddingOrdersRange_AndCouriersNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);

            IEnumerable<Courier> couriuers = null;

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await courierRepository.AddRangeAsync(couriuers));
        }

        [Fact]
        public async Task WhenAddingOrdersRange_AndCouriersEmpty_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);

            IEnumerable<Courier> couriuers = [];

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await courierRepository.AddRangeAsync(couriuers));
        }

        [Fact]
        public async Task WhenAddingOrdersRange_AndCouriersCorrect_ThenMethodShouldBeAddCouriers()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var firstCourierX = 7;
            var firstCourierY = 2;
            var firstCurrentCurierLocation = Location.Create(firstCourierX, firstCourierY).Value;

            var firstCourierSpeed = 2;
            var firstCourierName = "Vasiliy Maslakov";
            var firstCourier = Courier.Create(firstCourierSpeed, firstCourierName, firstCurrentCurierLocation).Value;

            var secondCourierX = 6;
            var secondCourierY = 4;
            var secondCurrentCurierLocation = Location.Create(secondCourierX, secondCourierY).Value;

            var secondCourierSpeed = 5;
            var secondCourierName = "Denis Yablochkov";
            var secondCourier = Courier.Create(secondCourierSpeed, secondCourierName, secondCurrentCurierLocation).Value;

            //Act
            await courierRepository.AddRangeAsync([firstCourier, secondCourier]);
            await unitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(await courierRepository.GetByIdAsync(firstCourier.Id), firstCourier);
            Assert.Equal(await courierRepository.GetByIdAsync(secondCourier.Id), secondCourier);
        }

        [Fact]
        public async Task WhenGettingCuorierById_AndCourierIdIsEmpty_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var courierId = Guid.Empty;

            //Act-Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await courierRepository.GetByIdAsync(courierId));
        }

        [Fact]
        public async Task WhenGettingCuorierById_AndCourierIsNotFound_ThenMethodShouldBeReturnNoValue()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var courierId = Guid.NewGuid();

            //Act-Assert
            var getCourierByIdResult = await courierRepository.GetByIdAsync(courierId);
            Assert.True(getCourierByIdResult.HasNoValue);
        }

        [Fact]
        public async Task WhenGettingCuorierById_AndCourierIsFound_ThenMethodShouldBeAddCourier()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var courierX = 7;
            var courierY = 2;
            var currentCurierLocation = Location.Create(courierX, courierY).Value;

            var courierSpeed = 2;
            var courierName = "Vasiliy Maslakov";
            var courier = Courier.Create(courierSpeed, courierName, currentCurierLocation).Value;

            await courierRepository.AddAsync(courier);
            await unitOfWork.SaveChangesAsync();

            //Act
            var courierInDatabase = await courierRepository.GetByIdAsync(courier.Id);

            //Assert
            Assert.Equal(courierInDatabase, courier);
        }

        [Fact]
        public async Task WhenUpdatingCourier_AndCourierIsNotHaveChanges_ThenMethodShouldntBeUpdateCourier()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var courierId = Guid.NewGuid();

            var courierX = 7;
            var courierY = 2;
            var currentCurierLocation = Location.Create(courierX, courierY).Value;

            var courierSpeed = 2;
            var courierName = "Vasiliy Maslakov";
            var courier = Courier.Create(courierSpeed, courierName, currentCurierLocation).Value;

            await courierRepository.AddAsync(courier);
            await unitOfWork.SaveChangesAsync();

            //Act
            courierRepository.Update(courier);

            //Assert
            var courierInDatabase = await courierRepository.GetByIdAsync(courier.Id);
            Assert.Equal(courierInDatabase.Value, courier);
        }

        [Fact]
        public async Task WhenUpdatingCourier_AndCourierIsHaveChanges_ThenMethodShoulBeUpdateCourier()
        {
            //Arrange
            var courierRepository = new CourierRepository(_databaseContext);
            var unitOfWork = new UnitOfWork(_databaseContext);

            var orderX = 1;
            var orderY = 2;
            var orderVolume = 4;
            var orderId = Guid.NewGuid();
            var orderLocation = Location.Create(orderX, orderY).Value;
            var order = Order.Create(orderId, orderLocation, orderVolume).Value;

            var courierX = 7;
            var courierY = 2;
            var currentCurierLocation = Location.Create(courierX, courierY).Value;

            var courierSpeed = 2;
            var courierId = Guid.NewGuid();
            var courierName = "Vasiliy Maslakov";
            var courier = Courier.Create(courierSpeed, courierName, currentCurierLocation).Value;

            courier.AddNewStoragePlace("name", 25);

            await courierRepository.AddAsync(courier);
            await unitOfWork.SaveChangesAsync();

            //Act
            courierRepository.Update(courier);

            //Assert
            var courierInDb = await courierRepository.GetByIdAsync(courier.Id);
            Assert.Equal(courierInDb.Value, courier);
        }
    }
}