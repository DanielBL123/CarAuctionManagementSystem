using CarAuction.Client.Service.Settings;
using CarAuction.Common.Tests.Unitary;
using CarAuction.Common.Tests.Unitary.Extensions;
using CarAuction.Dto;
using CarAuction.Dto.Request;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace CarAuction.Client.Service.Tests.Unitary;

public class ClientServiceUnitTests : BaseServiceTests<ClientService>
{

    private readonly IOptions<AuctionNotificationSettings> optionsStub;
    private readonly ClientService clientService;

    public ClientServiceUnitTests()
    {

        optionsStub = serviceCollection.AddSubstituteFor<IOptions<AuctionNotificationSettings>>();
        optionsStub.Value.Returns(GetMockAuctionNotificationSettings);

        serviceCollection.AddTransient<ClientService>();
        clientService = serviceCollection.BuildServiceProvider().GetRequiredService<ClientService>();
        
    }

    private static AuctionNotificationSettings GetMockAuctionNotificationSettings => new()
    {
        HubUrl = "http://localhost/hub",
        RetryCount = 1,
        RetryDelaySeconds = 1
    };


    [Fact]
    public async Task StopAsync_Should_Log_Stop_Message()
    {
        await clientService.StopAsync(CancellationToken.None);

        loggerStub.Received(1).Log(LogLevel.Debug, Arg.Any<EventId>(),Arg.Is<object>(o => o.ToString().Contains("Closing service")), null, Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task ExecuteAsync_Should_Login_And_Connect_WhenUserExists()
    {
        // Arrange

        using var input = new StringReader("user\npass\n");
        using var output = new StringWriter();
        Console.SetIn(input);
        Console.SetOut(output);

        authServiceStub.LoginAsync(Arg.Any<LoginUserRequest>())
            .Returns(Task.FromResult(new UserDto(1, "user")));

        optionsStub.Value.RetryCount = 1;
        var tokenSource = new CancellationTokenSource();

        // Act
        await clientService.StartAsync(tokenSource.Token);

        // Assert
        await authServiceStub.Received(1).LoginAsync(Arg.Any<LoginUserRequest>());

    }


    [Fact]
    public async Task ExecuteAsync_Should_Register_IfUserNotExists_And_ChooseToRegister()
    {

        using var input = new StringReader("user\npass\ny\n");
        using var output = new StringWriter();
        Console.SetIn(input);
        Console.SetOut(output);

        authServiceStub.LoginAsync(Arg.Any<LoginUserRequest>())
            .Returns(
                Task.FromResult<UserDto>(null!),
                Task.FromResult(new UserDto(1, "user"))
            );
        authServiceStub.RegisterAsync(Arg.Any<RegisterUserRequest>())
            .Returns(Task.FromResult(new UserDto(1, "user")));

        optionsStub.Value.RetryCount = 1;

        var tokenSource = new CancellationTokenSource();

        // Act
        await clientService.StartAsync(tokenSource.Token);

        loggerStub.Received().Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(o => o!.ToString()!.Contains($"The user user doesn't exists")), null, Arg.Any<Func<object, Exception?, string>>());

        loggerStub.Received().Log(LogLevel.Information, Arg.Any<EventId>(), Arg.Is<object>(o => o!.ToString()!.Contains("was sucessfully registered in the database")), null, Arg.Any<Func<object, Exception?, string>>());

        loggerStub.ReceivedWithAnyArgs().LogError(default, default, default, default(Exception), default(Func<object, Exception, string>));
    }


    [Fact]
    public async Task ExecuteAsync_Should_Cancel_IfTokenCancelledImmediately()
    {

        using var input = new StringReader("user\npass\n");
        using var output = new StringWriter();
        Console.SetIn(input);
        Console.SetOut(output);

        authServiceStub.LoginAsync(Arg.Any<LoginUserRequest>())
            .Returns(Task.FromResult(new UserDto(1, "user")));

        var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        await clientService.StartAsync(tokenSource.Token);

        await authServiceStub.DidNotReceiveWithAnyArgs().RegisterAsync(default!);
    }

}
