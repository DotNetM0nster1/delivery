using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Repositories
{
    public class RepositoryBase<TRepository>
    {
        private ApplicationDatabaseContext _databaseContext;

        protected TRepository Repository;
        protected UnitOfWork UnitOfWork;

        public async Task InitializeAsync(PostgreSqlContainer postgreSqlContainer)
        {
            await postgreSqlContainer.StartAsync();

            _databaseContext = CreateContext(postgreSqlContainer);
            Repository = (TRepository)Activator.CreateInstance(typeof(TRepository), _databaseContext);
            UnitOfWork = new UnitOfWork(_databaseContext);

            await _databaseContext.Database.MigrateAsync();
        }

        protected async Task ExecuteInNewDatabaseContextAsync(Func<TRepository, UnitOfWork, Task> action, PostgreSqlContainer postgreSqlContainer)
        {
            await using var databaseContext = CreateContext(postgreSqlContainer);

            var repository = (TRepository)Activator.CreateInstance(typeof(TRepository), databaseContext);
            var unitOfWork = new UnitOfWork(databaseContext);

            await action.Invoke(repository, unitOfWork);
        }

        private ApplicationDatabaseContext CreateContext(PostgreSqlContainer postgreSqlContainer)
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
                .UseNpgsql(
                    postgreSqlContainer.GetConnectionString(),
                    sqlOptions => { sqlOptions.MigrationsAssembly("DeliveryApp.Infrastructure"); }
                ).Options;

            return new ApplicationDatabaseContext(contextOptions);
        }

        public async Task DisposeAsync(PostgreSqlContainer postgreSqlContainer)
        {
            await postgreSqlContainer.DisposeAsync().AsTask();
            await _databaseContext.DisposeAsync();
        }
    }
}