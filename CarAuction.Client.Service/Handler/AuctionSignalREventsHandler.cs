using CarAuction.Dto;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace CarAuction.Client.Service.Handler;

public static class AuctionSignalREventsHandler
{
    public static void RegisterSignalREventsFromAuctions(HubConnection connection)
    {
        connection.On<object>("AuctionCreated", (auction) =>
        {
            Console.WriteLine($"[EVENT] New auction created: {JsonSerializer.Serialize(auction)}");
        });

        connection.On<object>("VehicleOpen", (vehicle) =>
        {
            Console.WriteLine($"[BIDDING OPEN] Vehicle now available: {JsonSerializer.Serialize(vehicle)}");
        });

        connection.On<int>("VehicleClosed", (vehicleId) =>
        {
            Console.WriteLine($"[BIDDING CLOSED] Vehicle {vehicleId} bidding closed.");
        });

        connection.On<int>("AuctionEnded", (auctionId) =>
        {
            Console.WriteLine($"[AUCTION ENDED] Auction {auctionId} has ended.");
        });

        connection.StartAsync();
    }

}
