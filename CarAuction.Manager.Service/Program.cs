using CarAuction.Common.Global.Extensions;
using CarAuction.Common.MappingProfiles;
using CarAuction.Manager.Service.Adapter;
using CarAuction.RealTime;
using CarAuction.RealTime.Interface;
using CarAuction.Sql.Context;
using CarAuction.Validation.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder()
        .ConfigureServices((hostContext, services) =>
        {
            var configuration = hostContext.Configuration;

            services.AddDbContext<CarAuctionSqlDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 36))
                ));

            services.AddAutoMapper(cfg => { cfg.AddProfile<AutoMapperProfile>(); });
            services.AddScoped<IAuctionNotifier, SignalRAuctionNotifier>();
            services.AddScoped<IBidNotifier, SignalRBidNotifier>();
            services.AddVehicleValidation();
            services.RegisterAllByAssembly<IVehicleTypeAdapter>()
                    .AddTransient<IVehicleTypeAdapterFactory, VehicleTypeAdapterFactory>();


            //services.AddHostedService<VehicleService>();
        });

