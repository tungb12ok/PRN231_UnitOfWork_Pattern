using JewelryAuctionBusiness;
using JewelryAuctionBusiness.Dto;
using JewelryAuctionData;
using JewelryAuctionData.Dto;
using JewelryAuctionData.Entity;
using JewelryAuctionWebAPI.BackgroundService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<Net1711_231_7_JewelryAuctionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));

// Register UnitOfWork and repositories
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<CustomerBusiness>();
builder.Services.AddScoped<RequestAuctionBusiness>();
builder.Services.AddScoped<PaymentBusiness>();
builder.Services.AddScoped<AuctionBusiness>();
builder.Services.AddScoped<BidderBusiness>();
// Add background services
// Add background services
builder.Services.AddHostedService<AuctionStatusUpdater>();
// Configure OData
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseAuthorization();
app.MapControllers();
app.Run();

IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<AuctionResult>("AuctionResults");
    builder.EntitySet<Customer>("Customers");
    builder.EntitySet<AuctionSection>("Auction");
    builder.EntitySet<Bidder>("Bidder");
    builder.EntitySet<Payment>("Payments");
    return builder.GetEdmModel();
}
