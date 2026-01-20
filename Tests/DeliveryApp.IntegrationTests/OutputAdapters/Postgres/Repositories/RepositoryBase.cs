using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using NSubstitute;
using MediatR;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Repositories
{
    public class RepositoryBase<TRepository> : IAsyncLifetime
    {
        protected IMediator Mediator = Substitute.For<IMediator>();
        private ApplicationDatabaseContext _databaseContext;
        protected TRepository Repository;
        protected UnitOfWork UnitOfWork;

        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:14.7")
            .WithDatabase("database")
            .WithPassword("password")
            .WithCleanUp(true)
            .Build();

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();

            _databaseContext = CreateContext();
            Repository = (TRepository)Activator.CreateInstance(typeof(TRepository), _databaseContext);

            UnitOfWork = new UnitOfWork(_databaseContext, Mediator);

            await _databaseContext.Database.MigrateAsync();
        }

        protected async Task ExecuteInNewDatabaseContextAsync(Func<TRepository, UnitOfWork, Task> action)
        {
            await using var databaseContext = CreateContext();

            var repository = (TRepository)Activator.CreateInstance(typeof(TRepository), databaseContext);
            var unitOfWork = new UnitOfWork(databaseContext, Mediator);

            await action.Invoke(repository, unitOfWork);
        }

        private ApplicationDatabaseContext CreateContext()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
                .UseNpgsql(
                    _postgreSqlContainer.GetConnectionString(),
                    sqlOptions => { sqlOptions.MigrationsAssembly("DeliveryApp.Infrastructure"); }
                ).Options;

            return new ApplicationDatabaseContext(contextOptions);
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync().AsTask();
            await _databaseContext.DisposeAsync();
        }
    }
}