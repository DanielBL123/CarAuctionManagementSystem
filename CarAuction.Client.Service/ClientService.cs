using CarAuction.Client.Service.Handler;
using CarAuction.Client.Service.Interface;
using CarAuction.Dto.Request;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;

namespace CarAuction.Client.Service;

public class ClientService(IAuthService authService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (await Login())
            {
                var connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5158/auctionHub")
               .Build();

                AuctionSignalREventsHandler.RegisterSignalREventsFromAuctions(connection);
            }
           
        }
        catch (Exception ex)
        {

        }
    }

    #region LOGIN
    async Task<bool> Login()
    {
        bool IsLogged = false;
        Console.Write("Enter username: ");
        var username = Console.ReadLine();

        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        var user = await authService.LoginAsync(new LoginUserRequest(username, password));

        if (user == null)
        {
            Console.WriteLine("User not found or password incorrect. Do you want to register? (y/n)");
            var choice = Console.ReadLine()?.ToLower();
            if (choice == "y")
            {
                try
                {
                    user = await authService.RegisterAsync(new RegisterUserRequest(username, password));
                    Console.WriteLine($"Registration successful! Welcome, {user.Username}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Exiting...");
                return false;
            }
        }
        else
        {
            Console.WriteLine($"Welcome back, {user.Username}!");
            IsLogged = true;
        }

        return IsLogged;
    }
    #endregion

}
