using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext
{
    public sealed class ApplicationMigrationDatabaseContext : IDesignTimeDbContextFactory<ApplicationDatabaseContext>
    {
        public ApplicationDatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDatabaseContext>();

            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User Id=username;Password=secret;Database=delivery;", op => 
            {
                op.MigrationsAssembly("DeliveryApp.Infrastructure"); 
            });

            return new ApplicationDatabaseContext(optionsBuilder.Options);
        }
    }
}
