using DeliveryApp.Core.Domain.Services.Distribute;
using DeliveryApp.Core.Ports;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Primitives;

namespace DeliveryApp.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCourierDistributorService(this IServiceCollection service)
        {
            return service.AddSingleton<ICourierDistributorService, CourierDistributorService>();
        }

        public static IServiceCollection AddCourierRepository(this IServiceCollection service)
        {
            return service.AddScoped<ICourierRepository, CourierRepository>();    
        }

        public static IServiceCollection AddOrderRepository(this IServiceCollection service)
        {
            return service.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection service)
        {
            return service.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddDatabaseContext(this IServiceCollection service, string connectionString)
        {
            return service.AddDbContext<ApplicationDatabaseContext>(options =>
            {
                options.UseNpgsql(connectionString, sqlOptions => 
                { 
                    sqlOptions.MigrationsAssembly("DeliveryApp.Infrastructure"); 
                });

                options.EnableSensitiveDataLogging();
            });
        }
    }
}