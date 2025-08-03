namespace CarAuction.Client.Service.Settings;

public class AuctionNotificationSettings
{
    public string HubUrl { get; set; } = null!;
    public int RetryCount { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 2;
}
