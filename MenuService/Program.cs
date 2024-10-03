using Microsoft.EntityFrameworkCore;
using MenuService.Data; // Assuming your DbContext is in the Data folder

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext for MenuService and configure it to use SQL Server
builder.Services.AddDbContext<MenuContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
