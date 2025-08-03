using CarAuction.Client.Service;
using CarAuction.Common.Global.Classes;
using CarAuction.Common.MappingProfiles;
using CarAuction.RealTime;
using CarAuction.RealTime.Interface;
using CarAuction.Sql.Context;
using CarAuction.Sql.Repositories.Classes;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

try
{
    Log.Logger = LogConfiguration.GetConfiguredLogger(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogConfig"));
    ApplicationLogging.LoggerFactory = SerilogLoggerFactoryExtensions.AddSerilog(new LoggerFactory(), Log.Logger);

    var host = CreateHostBuilder(args).Build();

    await host.RunAsync();
}catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}



static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;
            services.AddDbContext<CarAuctionSqlDbContext>(options =>
                options.UseMySql(
                    "server=sql7.freesqldatabase.com;port=3306;database=sql7792805;user=sql7792805;password=5wMHY7XKN7;",
                    new MySqlServerVersion(new Version(8, 0, 36))
                ));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBidNotifier, SignalRBidNotifier>();
            services.AddAutoMapper(cfg => { cfg.AddProfile<AutoMapperProfile>(); });

            services.Configure<AuctionNotificationSettings>(
                configuration.GetSection("AuctionNotificationSettings"));

            services.AddHostedService<ClientService>();
        });




