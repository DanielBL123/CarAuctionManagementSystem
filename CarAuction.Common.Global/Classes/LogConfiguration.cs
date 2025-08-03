using Microsoft.Extensions.Configuration;
using Serilog;

namespace CarAuction.Common.Global.Classes;

public static class LogConfiguration
{
    public static ILogger GetConfiguredLogger(string? pathOfSerilogConf = null) =>
        new LoggerConfiguration()
            .ReadFrom.Configuration(GetLogSettingFromConfigFile(pathOfSerilogConf ?? ""))
            .Enrich.FromLogContext()
            .CreateLogger();


    private static IConfigurationRoot GetLogSettingFromConfigFile(string path = "") =>
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(Path.Combine(path, "serilog.json"), optional: true, reloadOnChange: true)
            .Build();

}
