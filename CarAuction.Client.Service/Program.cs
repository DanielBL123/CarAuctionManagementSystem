using CarAuction.Client.Service;
using CarAuction.Client.Service.Handler;
using CarAuction.Client.Service.Interface;
using CarAuction.Common.MappingProfiles;
using CarAuction.Dto.Request;
using CarAuction.RealTime;
using CarAuction.RealTime.Interface;
using CarAuction.Sql.Context;
using CarAuction.Sql.Repositories.Classes;
using CarAuction.Sql.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();

services.AddDbContext<CarAuctionSqlDbContext>(options =>
                options.UseMySql(
                    "server=sql7.freesqldatabase.com;port=3306;database=sql7792805;user=sql7792805;password=5wMHY7XKN7;",
                    new MySqlServerVersion(new Version(8, 0, 36))
                ));
services.AddLogging();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IBidRepository, BidRepository>();
services.AddScoped<IAuctionRepository, AuctionRepository>();
services.AddScoped<IVehicleRepository, VehicleRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IBidNotifier, SignalRBidNotifier>();
services.AddAutoMapper(cfg => { cfg.AddProfile<AutoMapperProfile>(); });
var serviceProvider = services.BuildServiceProvider();
var authService = serviceProvider.GetRequiredService<IAuthService>();

Console.ReadLine();

