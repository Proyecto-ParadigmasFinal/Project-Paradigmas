using Microsoft.EntityFrameworkCore;
using Proyecto_Paradigmas.Services.Interfaces;
using Proyecto_Paradigmas.Services;
using ProyectoParadigmas.Database;
using ProyectoParadigmas.Services.CatalogItems;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HotelDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();
builder.Services.AddHttpClient<IPaypalServices, PaypalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
