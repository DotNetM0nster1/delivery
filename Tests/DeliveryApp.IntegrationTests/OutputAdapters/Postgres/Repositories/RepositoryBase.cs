using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Repositories
{
    public class RepositoryBase<TRepository> : IAsyncLifetime
    {
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
            UnitOfWork = new UnitOfWork(_databaseContext);

            await _databaseContext.Database.MigrateAsync();
        }

        protected async Task ExecuteInNewDatabaseContextAsync(Func<TRepository, UnitOfWork, Task> action)
        {
            await using var databaseContext = CreateContext();

            var repository = (TRepository)Activator.CreateInstance(typeof(TRepository), databaseContext);
            var unitOfWork = new UnitOfWork(databaseContext);

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