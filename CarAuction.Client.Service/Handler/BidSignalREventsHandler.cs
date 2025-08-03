using CarAuction.Model;
using Microsoft.AspNetCore.SignalR.Client;

namespace CarAuction.Client.Service.Handler;

public static class BidSignalREventsHandler
{
    public static void NotifyNewBid(HubConnection connection)
    {
        connection.On<Bid>("AuctionCreated", (bid) =>
        {
            Console.WriteLine($"[BID EVENT] : We have a new bid: {bid.Amount} for <{bid.User.Username}. Who wants to give more???");
        });
    }
}
