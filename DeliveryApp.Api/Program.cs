using DeliveryApp.Api.Extensions;
using DeliveryApp.Api;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["CONNECTION_STRING"];

// Health Checks
builder.Services.AddHealthChecks();
builder.Services.AddCourierDistributorService();
builder.Services.AddCourierRepository();
builder.Services.AddOrderRepository();
builder.Services.AddUnitOfWork();
builder.Services.AddAllActiveOrdersQuery(connectionString);
builder.Services.AddAllBusyCouriersModelProvider(connectionString);
builder.Services.AddDatabaseContext(connectionString);
builder.Services.AddMediator();
builder.Services.AddGetAllNotComplitedOrdersQuery();
builder.Services.AddGetAllBusyCouriersQuery();
builder.Services.AddMoveCourierCommand();
builder.Services.AddAssignOrderCommand();
builder.Services.AddCreateOrderCommand();
builder.Services.AddCronJobs();
builder.Services.AddHttpHandlers();
builder.Services.AddSwagger();

// Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin(); // Не делайте так в проде!
        });
});

// Configuration
builder.Services.ConfigureOptions<SettingsSetup>();
var app = builder.Build();

// -----------------------------------
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHealthChecks("/health");
app.UseRouting();

// Apply Migrations
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     db.Database.Migrate();
// }

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSwagger(c => { c.RouteTemplate = "openapi/{documentName}/openapi.json"; })
    .UseSwaggerUI(options =>
    {
        options.RoutePrefix = "openapi";
        options.SwaggerEndpoint("/openapi/1.0.0/openapi.json", "Swagger Delivery Service");
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint("/openapi-original.json", "Swagger Delivery Service");
    });

app.UseCors();
app.UseEndpoints(endpoints => { _ = endpoints.MapControllers(); });

app.Run();