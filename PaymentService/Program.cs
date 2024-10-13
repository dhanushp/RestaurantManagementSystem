using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DependencyInjection;
using PaymentService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext for PaymentService and configure it to use SQL Server
//builder.Services.AddDbContext<PaymentDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();

// Add Swagger for API testing and documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddSignalR();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<PaymentHub>("/paymentHub");

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.UseInfrastructurePolicy();

app.MapControllers();

app.Run();
