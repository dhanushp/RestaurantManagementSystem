using Microsoft.EntityFrameworkCore;
using OrderService.Data; // Assuming your DbContext is in the Data folder
using OrderService.Interfaces; // Make sure to include your service interfaces
using OrderService.Repositories; // Include the OrderRepository
using OrderService.HttpClients; // Include HttpClients for external services
using OrderService.Models; // If needed for any model
using MenuService.Interfaces; // Assuming you have interfaces for MenuService
using UserService.Interfaces; // Assuming you have interfaces for UserService
using UserService.Repositories; // User repository if used
using MenuService.Repositories; // Menu repository if used
using MenuService.Services; // Menu service if used
using UserService.Data; // Assuming you have DbContext for UserService
using MenuService.Data; // Assuming you have DbContext for MenuService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
// Add DbContext for OrderService and configure it to use SQL Server
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register OrderService and its dependencies
builder.Services.AddScoped<IOrderService, OrderServiced>(); // Register the OrderService
builder.Services.AddScoped<IOrderRepository, OrderRepository>(); // Register the OrderRepository
builder.Services.AddScoped<IOrderSummaryRepository, OrderSummaryRepository>();
builder.Services.AddScoped<IOrderSummaryService, OrderSummaryService>();
// Register UserService dependencies
//builder.Services.AddScoped<IUser, UserRepository>(); // Register IUser and its implementation

// Register MenuItem dependencies
//builder.Services.AddScoped<IMenuItem, MenuItemRepository>(); // Assuming MenuItemRepository implements IMenuItem

// Register HttpClient for UserService
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
