using CarAuction.Common.MappingProfiles;
using CarAuction.Manager.Service;
using CarAuction.Manager.Service.Adapter;
using CarAuction.RealTime;
using CarAuction.RealTime.Interface;
using CarAuction.Sql.Context;
using CarAuction.Sql.Repositories.Classes;
using CarAuction.Sql.Repositories.Interfaces;
using CarAuction.Common.Global.Extensions;
using CarAuction.Validation.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CarAuctionSqlDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 36))));
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddVehicleValidation();
builder.Services.RegisterAllByAssembly<IVehicleTypeAdapter>()
                    .AddTransient<IVehicleTypeAdapterFactory, VehicleTypeAdapterFactory>();

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<AutoMapperProfile>(); });

builder.Services.AddSignalR();
builder.Services.AddScoped<IAuctionNotifier, SignalRAuctionNotifier>();
builder.Services.AddScoped<IBidNotifier, SignalRBidNotifier>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers();


var app = builder.Build();

app.MapHub<AuctionHub>("/auctionHub");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowAll");
app.Run();
