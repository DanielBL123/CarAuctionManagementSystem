using CarAuction.Client.Service.Handler;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace CarAuction.Client.Service;

public class ClientService(IAuthService authService, IOptions<AuctionNotificationSettings> options, ILogger<ClientService> logger) : BackgroundService
{

    private AuctionNotificationSettings AuctionNotificationSettings => options.Value;

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Starting service to notificate users about new auctions and new bids");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Closing service to notificate users about new auctions and new bids");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            bool isLogged = false;

            do
            {
                isLogged = await ValidateLogin();

            } while (isLogged == false);

            await ConnectToHub();
            
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("There was an error in the application: " + ex.Message);
            logger.LogError(ex, "Error while trying to login");
        }
        catch (ConnectionAbortedException ex)
        {
            Console.WriteLine("Error while connecting to the Hub: " + ex.Message);
            logger.LogError(ex, "Maximum retry attempts reached. Stopping service.");
        }
    }

    private async Task<bool> ValidateLogin()
    {
        (var username, var password) = EnterCredentials();
        var user = await authService.LoginAsync(new LoginUserRequest(username!, password!));

        if (user == null)
        {
            logger.LogInformation($"The user {username} doesn't exists!");

            Console.WriteLine("User not found or password incorrect. Do you want to register? (y/n)");

            var choice = Console.ReadLine()?.ToLower();

            if (choice == "y")
            {
                await RegisterUser(username, password);
                logger.LogInformation("User {username} was sucessfully registered in the database", username);
                return await authService.LoginAsync(new LoginUserRequest(username!, password!)) != null;
            }
            else
            {
                return false;

            }
        }
        else
        {
            Console.WriteLine($"Welcome back {username}");
            return true;
        }

    }

    private (string username, string password) EnterCredentials()
    {
        Console.Write("Enter username: ");
        var username = Console.ReadLine();

        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        return (username!, password!);
    }

    private async Task RegisterUser(string username, string password) =>
        await authService.RegisterAsync(new RegisterUserRequest(username, password));


    private async Task ConnectToHub()
    {
        var connection = new HubConnectionBuilder()
                .WithUrl(AuctionNotificationSettings.HubUrl)
                .Build();

        AuctionSignalREventsHandler.RegisterSignalREventsFromAuctions(connection);
        await connection.StartAsync();

        logger.LogInformation("Connected to SignalR Hub at {HubUrl}", AuctionNotificationSettings.HubUrl);

    } 

        //var connection = new HubConnectionBuilder()
        //       .WithUrl("http://localhost:5158/auctionHub")
        //       .Build();

        //AuctionSignalREventsHandler.RegisterSignalREventsFromAuctions(connection);



        //int retries = 0;
        //try
        //{


        //await connection.StartAsync(stoppingToken);


        //}
        //catch (Exception ex)
        //{
        //    //retries++;
        //    logger.LogWarning(ex, "Failed to connect to Hub. Attempt {Retry}/{MaxRetries}", retries,AuctionNotificationSettings.RetryCount);

        //    //if (retries >= AuctionNotificationSettings.RetryCount)
        //    //{
        //    //    throw new ConnectionAbortedException("Maximum retry attempts reached. We couldn't connectyou o the HUB");
        //    //}

        //    //await Task.Delay(TimeSpan.FromSecondsAuctionNotificationSettings.RetryDelaySeconds),stoppingToken);
        //}

    
}
