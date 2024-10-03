using Microsoft.EntityFrameworkCore;
using ReviewService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Configure DbContext to use SQL Server and read the connection string from appsettings.json
builder.Services.AddDbContext<ReviewContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI support for documentation and testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline

// Use Swagger in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
