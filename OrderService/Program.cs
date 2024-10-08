using Microsoft.EntityFrameworkCore;
using OrderService.Data; // Assuming your DbContext is in the Data folder
using OrderService.Interfaces; // Make sure to include your service interfaces// Include the OrderService
using OrderService.Repositories;
using OrderService.HttpClients;               // Include the OrderRepository
using OrderService.Models; // If needed for any model
using MenuService.Interfaces;
using UserService.Interfaces;
using UserService.Repositories;
using MenuService.Repositories;
using MenuService.Services;
using UserService.Data;
using MenuService.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext for OrderService and configure it to use SQL Server
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Replace with your connection string name

builder.Services.AddDbContext<MenuContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services
builder.Services.AddScoped<IOrderService,OrderServiced>(); // Register the OrderService
builder.Services.AddScoped<IOrderRepository, OrderRepository>(); // Register the OrderRepository
// Register services
builder.Services.AddScoped<IUser, UserRepository>(); // Register IUser and its implementation
builder.Services.AddScoped<IOrderService, OrderServiced>(); // Register OrderServiced
// Assuming MenuItemRepository implements IMenuItem
builder.Services.AddScoped<IMenuItem, MenuItemRepository>();


builder.Services.AddTransient<UserHttpClient>(sp =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["UserService:BaseUrl"])
    };
    return new UserHttpClient(httpClient);
});
builder.Services.AddTransient<IUsers>(sp => sp.GetRequiredService<UserHttpClient>());

// Register HttpClient for MenuService
builder.Services.AddTransient<MenuItemHttpClient>(sp =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["MenuService:BaseUrl"])
    };
    return new MenuItemHttpClient(httpClient);
});

builder.Services.AddTransient<IMenuItems>(sp => sp.GetRequiredService<MenuItemHttpClient>());
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
