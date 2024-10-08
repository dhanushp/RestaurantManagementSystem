using Microsoft.EntityFrameworkCore;
using MenuService.Data; // Assuming your DbContext is in the Data folder
using MenuService.Services; // Add this for MenuItemService
using MenuService.Interfaces; // Add this for IMenuItem
using MenuService.Repositories;
using MenuService.DependencyInjection; // Add this for MenuItemRepository

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add DbContext for MenuService and configure it to use SQL Server
//builder.Services.AddDbContext<MenuContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Add controllers
builder.Services.AddControllers();

// Add Swagger for API testing and documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddInfrastructureService(builder.Configuration);


var app = builder.Build();

// Apply migrations and seed data in one scope
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MenuContext>();

    // Apply any pending migrations
    dbContext.Database.Migrate();

    // Seed data - call your combined seed method here if applicable
    SeedData(dbContext);
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseInfrastructurePolicy();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Method to seed data
void SeedData(MenuContext dbContext)
{
    try
    {
        // Check if categories exist, if not, seed them
        if (!dbContext.Categories.Any())
        {
            dbContext.SeedCategories(); // Seed categories
        }

        // Now seed menu items
        if (!dbContext.MenuItems.Any())
        {
            dbContext.SeedMenuItems(); // Seed menu items
        }

        // Call SaveChanges to commit the transaction
        dbContext.SaveChanges();
    }
    catch (Exception ex)
    {
        // Log the error (consider using a logging framework)
        Console.WriteLine($"Error seeding data: {ex.Message}");
    }
}
