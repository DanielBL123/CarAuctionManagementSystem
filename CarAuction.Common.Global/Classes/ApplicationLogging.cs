using Microsoft.Extensions.Logging;

namespace CarAuction.Common.Global.Classes;

public static class ApplicationLogging
{
    public static ILoggerFactory LoggerFactory { get; set; } = null!;
}
