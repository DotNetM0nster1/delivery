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
    public class CourierRepositoryTest : RepositoryBase<CourierRepository>
    {
        [Fact]
        public async Task WhenAddingCourier_AndCourierIsNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange-Act
            Func<Task> func = async () => 
            { 
                await Repository.AddAsync(null);
            };

            //Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenAddingCourier_AndCourierIsNotNull_ThenMethodShouldBeAddCourier()
        {
            //Arrange
            var courier = Courier.Create(3, "Kirill Mikrov", Location.Create(5, 9).Value).Value;

            //Act
            await Repository.AddAsync(courier);
            await UnitOfWork.SaveChangesAsync();

            //Assert
            var getCourierByIdResult = await Repository.GetByIdAsync(courier.Id);
            getCourierByIdResult.Value.Should().BeEquivalentTo(courier, opt => opt.ComparingByMembers<Courier>());
        }

        [Fact]
        public async Task WhenGettingAllFreeCouriers_AndFreeCouriersNotFound_ThenMethodShouldBeReturnZeroCountCouriers()
        {
            //Arrange
            var courier = Courier.Create(3, "Kirill Mikrov", Location.Create(5, 9).Value).Value;
            var order = Order.Create(Guid.NewGuid(), Location.Create(3, 2).Value, 7).Value;

            courier.TakeOrder(order);

            await Repository.AddAsync(courier);
            await UnitOfWork.SaveChangesAsync();

            //Act-Assert
            var getAllCouriuersResult = await Repository.GetAllFreeCouriersAsync();
            getAllCouriuersResult.Should().HaveCount(0);
        }

        [Fact]
        public async Task WhenGettingAllFreeCouriers_AndFreeCouriersFound_ThenMethodShouldBeReturnFreeCouriers()
        {
            //Arrange
            Courier firstCourier = Courier.Create(3, "Kirill Mikrov", Location.Create(5, 9).Value).Value;
            Courier secondCourier = Courier.Create(5, "Alexandr Ivin", Location.Create(2, 2).Value).Value;

            await ExecuteInNewDatabaseContextAsync(async (repository, unitOfWork) => 
            {
                await repository.AddRangeAsync([firstCourier, secondCourier]);
                await unitOfWork.SaveChangesAsync();
            });

            //Act
            List<Courier> allFreeCouriersResult = null;

            await ExecuteInNewDatabaseContextAsync(async (repository, unitOfWork) => 
            {
                allFreeCouriersResult = await repository.GetAllFreeCouriersAsync();
            });

            //Assert
            allFreeCouriersResult.Count.Should().Be(2);
            allFreeCouriersResult.Should().Contain(firstCourier);
            allFreeCouriersResult.Should().Contain(secondCourier);
        }

        [Fact]
        public async Task WhenAddingOrdersRange_AndCouriersNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange-Act
            Func<Task> func = async () =>
            {
                await Repository.AddRangeAsync(null);
            };

            //Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenAddingOrdersRange_AndCouriersEmpty_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange-Act
            Func<Task> func = async () =>
            {
                await Repository.AddRangeAsync([]);
            };

            //Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenAddingOrdersRange_AndCouriersCorrect_ThenMethodShouldBeAddCouriers()
        {
            //Arrange
            var firstCourier = Courier.Create(2, "Vasiliy Maslakov", Location.Create(7, 2).Value).Value;
            var secondCourier = Courier.Create(5, "Denis Yablochkov", Location.Create(6, 4).Value).Value;

            //Act
            await Repository.AddRangeAsync([firstCourier, secondCourier]);
            await UnitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(await Repository.GetByIdAsync(firstCourier.Id), firstCourier);
            Assert.Equal(await Repository.GetByIdAsync(secondCourier.Id), secondCourier);
        }

        [Fact]
        public async Task WhenGettingCuorierById_AndCourierIdIsEmpty_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange-Act
            Func<Task> func = async () =>
            {
                await Repository.GetByIdAsync(Guid.Empty);
            };

            //Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task WhenGettingCuorierById_AndCourierIsNotFound_ThenMethodShouldBeReturnNoValue()
        {
            //Arrange-Act-Assert
            var getCourierByIdResult = await Repository.GetByIdAsync(Guid.NewGuid());
            getCourierByIdResult.HasNoValue.Should().Be(true);
        }

        [Fact]
        public async Task WhenGettingCuorierById_AndCourierIsFound_ThenMethodShouldBeAddCourier()
        {
            //Arrange
            var courier = Courier.Create(2, "Vasiliy Maslakov", Location.Create(7, 2).Value).Value;

            await Repository.AddAsync(courier);
            await UnitOfWork.SaveChangesAsync();

            //Act
            var courierInDatabase = await Repository.GetByIdAsync(courier.Id);

            //Assert
            courierInDatabase.Value.Should().BeSameAs(courier);
        }

        [Fact]
        public async Task WhenUpdatingCourier_AndCourierIsNotHaveChanges_ThenMethodShouldntBeUpdateCourier()
        {
            //Arrange
            Courier courier = Courier.Create(2, "Maxim Maslakov", Location.Create(7, 2).Value).Value; 

            await ExecuteInNewDatabaseContextAsync(async (repository, unitOfWork) =>
            {
                await repository.AddAsync(courier);
                await unitOfWork.SaveChangesAsync();
            });

            //Act
            Repository.Update(courier);

            //Assert
            var courierInDatabase = await Repository.GetByIdAsync(courier.Id);
            courierInDatabase.Value.Should().BeSameAs(courier);
        }

        [Fact]
        public async Task WhenUpdatingCourier_AndCourierIsHaveChanges_ThenMethodShoulBeUpdateCourier()
        {
            //Arrange
            Courier courier = Courier.Create(2, "Vasiliy Maximov", Location.Create(7, 2).Value).Value; ;
            var order = Order.Create(Guid.NewGuid(), Location.Create(1, 2).Value, 4).Value;
            var storagePlace = StoragePlace.Create("SomeName", 25).Value;

            await ExecuteInNewDatabaseContextAsync(async (repository, unitOfWork) => 
            {
                await repository.AddAsync(courier);
                await unitOfWork.SaveChangesAsync();
            });

            //Act
            await ExecuteInNewDatabaseContextAsync(async (repository, unitOfWork) =>
            {
                courier.Move(Location.Create(1,2).Value);
                repository.Update(courier);
                await unitOfWork.SaveChangesAsync();
            });

            //Assert
            var courierInDb = await Repository.GetByIdAsync(courier.Id);
            courierInDb.Value.Name.Should().Be("Vasiliy Maximov");
            courierInDb.Value.Location.X.Should().Be(5);
            courierInDb.Value.Location.Y.Should().Be(2);
        }

        [Fact]
        public void WhenUpdatingCourier_AndCourierIdIsNull_ThenMethodShouldBeThrowArgumentNullException()
        {
            //Arrange-Act
            Action action = () =>
            {
                Repository.Update(null);
            };

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}