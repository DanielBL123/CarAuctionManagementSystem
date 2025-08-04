using CarAuction.Dto;
using CarAuction.RealTime.Interface;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text;

namespace CarAuction.RealTime;

public class SignalRBidNotifier : IBidNotifier
{
    public async Task NotifyBidPlacedAsync(BidDto bid)
    {
        using var client = new HttpClient();
        var content = new StringContent(JsonConvert.SerializeObject(bid), Encoding.UTF8, "application/json");
        await client.PostAsync("http://localhost:5158/notify/BidPlaced", content);
    }
}
