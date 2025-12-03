using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Providers.CourierProvider.GetAllBusy;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Providers.OrderProvider.GetAllActive;
using DeliveryApp.Core.Application.UseCases.Queries.OrderQuery.GetAllNotComplitedOrders;
using DeliveryApp.Core.Application.UseCases.Queries.CourierQuery.GetAllBusyCouriers;
using DeliveryApp.Core.Application.UseCases.Commands.CourierCommands.MoveCouriers;
using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.AssignOrder;
using DeliveryApp.Core.Application.UseCases.Commands.OrderCommands.CreateOrder;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.ApplicationContext;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;
using DeliveryApp.Api.Adapters.Http.Contract.src.OpenApi.Formatters;
using DeliveryApp.Api.Adapters.Http.Contract.src.OpenApi.Filters;
using DeliveryApp.Api.Adapters.Http.Contract.src.OpenApi.OpenApi;
using DeliveryApp.Infrastructure.OutputAdapters.Grpc.GeoService;
using DeliveryApp.Infrastructure.OutputAdapters.Postgres;
using DeliveryApp.Api.InputAdapters.BackgroundJobs;
using DeliveryApp.Core.Domain.Services.Distribute;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using Newtonsoft.Json.Converters;
using Microsoft.OpenApi.Models;
using DeliveryApp.Core.Ports;
using System.Reflection;
using Primitives;
using MediatR;
using Quartz;

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
            return services.AddScoped<IAllActiveOrdersResult, AllActiveOrdersModelProvider>();
        }

        public static IServiceCollection AddAllBusyCouriersModelProvider(this IServiceCollection services, string connectionString)
        {
            return services.AddScoped<IAllBusyCouriersModelProvider, AllBusyCouriersModelProvider>();
        }

        public static IServiceCollection AddMoveCourierCommand(this IServiceCollection services)
        {
            return services.AddScoped<IRequestHandler<MoveCouriersCommand, UnitResult<Error>>, MoveCouriersHandler>();
        }

        public static IServiceCollection AddAssignOrderCommand(this IServiceCollection services)
        {
            return services.AddScoped<IRequestHandler<AssignOrdersCommand, UnitResult<Error>>, AssignOrdersHandler>();
        }

        public static IServiceCollection AddCreateOrderCommand(this IServiceCollection services)
        {
            return services.AddScoped<IRequestHandler<CreateOrderCommand, UnitResult<Error>>, CreateOrderHandler>();
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        { 
            return services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        public static IServiceCollection AddGetAllNotComplitedOrdersQuery(this IServiceCollection services)
        {
            return services.AddScoped<IRequestHandler<GetAllNotComplitedOrdersQuery, GetAllNotComplitedOrdersResponse>, GetAllNotComplitedOrdersHandler>();
        }

        public static IServiceCollection AddGetAllBusyCouriersQuery(this IServiceCollection services)
        {
            return services.AddScoped<IRequestHandler<GetAllBusyCouriersQuery, GetAllBusyCouriersResponse>, GetAllBusyCouriersHandler>();
        }

        public static IServiceCollection AddGrpcClient(this IServiceCollection services)
        {
            return services.AddScoped<IGeoClient, GeoClient>();
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

        public static IServiceCollection AddCronJobs(this IServiceCollection services)
        {
            services.AddQuartz(configure =>
            {
                var assignOrdersJobKey = new JobKey(nameof(AssignOrdersJob));
                var moveCouriersJobKey = new JobKey(nameof(MoveCouriersJob));

                configure
                    .AddJob<AssignOrdersJob>(assignOrdersJobKey)
                    .AddTrigger(trigger => trigger.ForJob(assignOrdersJobKey)
                        .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(1)
                        .RepeatForever()))
                    .AddJob<MoveCouriersJob>(moveCouriersJobKey)
                    .AddTrigger(trigger => trigger.ForJob(moveCouriersJobKey)
                        .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(2)
                        .RepeatForever()));
            });

            services.AddQuartzHostedService();

            return services;
        }

        public static IServiceCollection AddHttpHandlers(this IServiceCollection services)
        {
            services.AddControllers(options => { options.InputFormatters.Insert(0, new InputFormatterStream()); })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    });
                });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("1.0.0", new OpenApiInfo
                {
                    Title = "Delivery Service",
                    Description = "Отвечает за диспетчеризацию доставки",
                    Contact = new OpenApiContact
                    {
                        Name = "Kirill Vetchinkin",
                        Url = new Uri("https://microarch.ru"),
                        Email = "info@microarch.ru"
                    }
                });

                options.CustomSchemaIds(type => type.FriendlyId(true));
                options.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly()?.GetName().Name}.xml");
                options.DocumentFilter<BasePathFilter>("");
                options.OperationFilter<GeneratePathParamsValidationFilter>();
            });

            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}