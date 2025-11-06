using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Queries.Courier.GetAllBusy;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Queries.Order.GetAllActive;
using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.CreateOrder;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveCouriers;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using DeliveryApp.Core.Domain.Services.Distribute;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Ports;
using Primitives;
using MediatR;
using DeliveryApp.Core.Application.UseCases.Commands.Order.AssignOrder;

namespace DeliveryApp.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCourierDistributorService(this IServiceCollection services)
        {
            return services.AddSingleton<ICourierDistributorService, CourierDistributorService>();
        }

        public static IServiceCollection AddCourierRepository(this IServiceCollection services)
        {
            return services.AddScoped<ICourierRepository, CourierRepository>();    
        }

        public static IServiceCollection AddOrderRepository(this IServiceCollection services)
        {
            return services.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddAllActiveOrdersQuery(this IServiceCollection services, string connectionString)
        {
            return services.AddScoped<IAllActiveOrdersResult>(x => new AllActiveOrdersResult(connectionString));
        }

        public static IServiceCollection AddAllBusyCouriersQuery(this IServiceCollection services, string connectionString)
        {
            return services.AddScoped<IAllBusyCouriersModelProvider>(x => new AllBusyCouriersModelProvider(connectionString));
        }

        public static IServiceCollection AddMoveCourierCommand(this IServiceCollection services)
        {
            return services.AddScoped<IRequestHandler<MoveCouriersCommand, UnitResult<Error>>, MoveCouriersHandler>();
        }

        public static IServiceCollection AddAssignOrderCommand(this IServiceCollection services)
        {
            return services.AddScoped<IRequestHandler<AssignOrderCommand, UnitResult<Error>>, AssignOrderHandler>();
        }

        public static IServiceCollection AddCreateOrderCommand(this IServiceCollection services)
        {
            return services.AddScoped<IRequestHandler<CreateOrderCommand, UnitResult<Error>>, CreateOrderHandler>();
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