using System.Text.Json.Serialization;
using JewelryAuctionBusiness;
using JewelryAuctionBusiness.AutoMap;
using JewelryAuctionBusiness.Dto;
using JewelryAuctionData;
using JewelryAuctionData.Dto;
using JewelryAuctionData.Entity;
using JewelryAuctionWebAPI.BackgroundService;
using JewelryAuctionWebAPI.Controllers;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddOData(opt =>
{
    opt.Select()
        .Filter()
        .Count()
        .OrderBy()
        .Expand()
        .SetMaxTop(100);
    opt.AddRouteComponents("odata", GetEdmModel());
});


// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "JewelryAuctionWebAPI", Version = "v1" });
    c.EnableAnnotations();
});

// Add DbContext
builder.Services.AddDbContext<Net1711_231_7_JewelryAuctionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));

// Register UnitOfWork and business services
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<CustomerBusiness>();
builder.Services.AddScoped<RequestAuctionBusiness>();
builder.Services.AddScoped<PaymentBusiness>();
builder.Services.AddScoped<AuctionBusiness>();
builder.Services.AddScoped<BidderBusiness>();
builder.Services.AddScoped<AuctionResultBusiness>();

// Add background services
builder.Services.AddHostedService<AuctionStatusUpdater>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// // Configure JSON options
// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//     {
//         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//         options.JsonSerializerOptions.WriteIndented = fa;
//     });

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();
app.Run();

IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();

    builder.EntitySet<AuctionResultDto>("AuctionResults");
    builder.EntitySet<AuctionSectionDto>("Auction");
    builder.EntitySet<BidderDto>("Bidders");
    builder.EntitySet<CustomerDTO>("Customers");
    builder.EntitySet<JewelryDTO>("Jewelry");
    builder.EntitySet<CompanyDTO>("Companies");
    builder.EntitySet<RequestAuctionDTO>("AuctionRequest");
    builder.EntitySet<RequestAuctionDetailsDto>("RequestAuctionDetail");
    builder.EntitySet<PaymentDto>("Payments");

    return builder.GetEdmModel();
}

