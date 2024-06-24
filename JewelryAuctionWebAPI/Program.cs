using JewelryAuctionBusiness;
using JewelryAuctionData.Repository;
using JewelryAuctionData;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using JewelryAuctionData.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add DBContext
builder.Services.AddDbContext<Net1711_231_7_JewelryAuctionContext>();
// Register UnitOfWork and repositories
builder.Services.AddScoped<UnitOfWork>(); 
builder.Services.AddScoped<CustomerBusiness>();

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
