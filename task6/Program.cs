using task6.Models;
using task6.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
