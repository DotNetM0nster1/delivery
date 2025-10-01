using DeliveryApp.Api;
using DeliveryApp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["CONNECTION_STRING"];

// Health Checks
builder.Services.AddHealthChecks();
builder.Services.AddCourierDistributorService();
builder.Services.AddCourierRepository();
builder.Services.AddOrderRepository();
builder.Services.AddUnitOfWork();
builder.Services.AddDatabaseContext(connectionString);

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

app.Run();