using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using DeliveryApp.Infrastructure;
using Testcontainers.PostgreSql;
using NSubstitute;
using MediatR;
using Xunit;

namespace DeliveryApp.IntegrationTests.OutputAdapters.Postgres.Providers
{
    public class ProviderBase<TProvider> : IAsyncLifetime
    {
        protected IMediator Mediator = Substitute.For<IMediator>();
        protected ApplicationDatabaseContext DatabaseContext;
        protected TProvider ModelProvider;
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

            var connectionString = _postgreSqlContainer.GetConnectionString();
            var options = GetOptions(connectionString);

            ModelProvider = (TProvider)Activator.CreateInstance(typeof(TProvider), options);

            DatabaseContext = CreateContext();
            UnitOfWork = new UnitOfWork(DatabaseContext, Mediator);

            await DatabaseContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync().AsTask();
            await DatabaseContext.DisposeAsync();
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

        private IOptions<Settings> GetOptions(string connectionString)
        {
            Settings settings = new()
            {
                ConnectionString = connectionString
            };

            return Options.Create(settings);
        }
    }
}