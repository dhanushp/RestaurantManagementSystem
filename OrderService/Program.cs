using Microsoft.EntityFrameworkCore;
using OrderService.Data; // Assuming your DbContext is in the Data folder
using OrderService.Interfaces; // Make sure to include your service interfaces// Include the OrderService
using OrderService.Repositories; // Include the OrderRepository
using OrderService.Models; // If needed for any model
using MenuService.Interfaces;
using UserService.Interfaces;
using OrderService.Respositories;
using UserService.Repositories;
using MenuService.Repositories;
using MenuService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext for OrderService and configure it to use SQL Server
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services
builder.Services.AddScoped<IOrderService, OrderServiced>(); // Register the OrderService
builder.Services.AddScoped<IOrderRepository, OrderRepository>(); // Register the OrderRepository
// Register HttpClient for UserService
/*builder.Services.AddHttpClient<IUser, UserHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["UserService:BaseUrl"]);
});

// Register HttpClient for MenuService
builder.Services.AddHttpClient<IMenuItem, MenuItemHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MenuService:BaseUrl"]);
});*/

// Add controllers
builder.Services.AddControllers();

// Add Swagger for API testing and documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
