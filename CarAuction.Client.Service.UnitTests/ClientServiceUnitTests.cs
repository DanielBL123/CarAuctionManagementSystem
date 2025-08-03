//using CarAuction.Client.Service.Interface;
//using CarAuction.Common.UnitTests;
//using CarAuction.Dto.Request;
//using CarAuction.Model;
//using NSubstitute;
//using NSubstitute.Core;
//using NSubstitute.ExceptionExtensions;

//namespace CarAuction.Client.Service.UnitTests;

//public class ClientServiceUnitTests
//{
//    private readonly IAuthService _authService;

//    public ClientServiceUnitTests()
//    {

//        _authService = <IAuthService>();
//    }

//    private ClientService CreateService()
//    {
//        return new ClientService(_authService);
//    }

//    private static void SimulateConsoleInput(params string[] inputs)
//    {
//        var inputString = string.Join(Environment.NewLine, inputs);
//        Console.SetIn(new StringReader(inputString));
//        Console.SetOut(new StringWriter());
//    }

//    private static async Task<bool> InvokeLogin(ClientService service)
//    {
//        var method = typeof(ClientService)
//            .GetMethod("Login", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//        return await (Task<bool>)method.Invoke(service, null);
//    }


//    [Fact]
//    public async Task Login_ShouldReturnTrue_WhenUserExists()
//    {

//        var user = MockData.MockUsers()[0];
//        // Arrange
//        _authService.LoginAsync(Arg.Any<LoginUserRequest>()).Returns(user);

//        var service = CreateService();
//        SimulateConsoleInput("username", "password");

//        // Act
//        var result = await InvokeLogin(service);

//        // Assert
//        Assert.True(result);
//    }

//    [Fact]
//    public async Task Login_ShouldRegister_WhenLoginFails_AndUserChoosesYes()
//    {
//        // Arrange
//        _authService.LoginAsync(Arg.Any<LoginUserRequest>())
//                    .Returns(Task.FromResult<User>(null));
//        _authService.RegisterAsync(Arg.Any<RegisterUserRequest>())
//                    .Returns(Task.FromResult(GetTestUser()));

//        var service = CreateService();
//        SimulateConsoleInput("username", "password", "y");

//        // Act
//        var result = await InvokeLogin(service);

//        // Assert
//        Assert.True(result);
//    }

//    [Fact]
//    public async Task Login_ShouldReturnFalse_WhenRegistrationThrowsException()
//    {
//        // Arrange
//        _authService.LoginAsync(Arg.Any<LoginUserRequest>())
//                    .Returns(Task.FromResult<User>(null));
//        _authService.RegisterAsync(Arg.Any<RegisterUserRequest>())
//                    .Throws(new InvalidOperationException("Registration failed"));

//        var service = CreateService();
//        SimulateConsoleInput("username", "password", "y");

//        // Act
//        var result = await InvokeLogin(service);

//        // Assert
//        Assert.False(result);
//    }

//    [Fact]
//    public async Task Login_ShouldReturnFalse_WhenLoginFails_AndUserChoosesNo()
//    {
//        // Arrange
//        _authService.LoginAsync(Arg.Any<LoginUserRequest>())
//                    .Returns(Task.FromResult<User>(null));

//        var service = CreateService();
//        SimulateConsoleInput("username", "password", "n");

//        // Act
//        var result = await InvokeLogin(service);

//        // Assert
//        Assert.False(result);
//    }


//    [Fact]
//    public async Task ExecuteAsync_ShouldCallRegisterEvents_WhenLoginSucceeds()
//    {
//        // Arrange
//        _authService.LoginAsync(Arg.Any<LoginUserRequest>())
//                    .Returns(Task.FromResult(GetTestUser()));
//        var service = CreateService();
//        SimulateConsoleInput("username", "password");

//        // Act
//        await service.StartAsync(CancellationToken.None);
//        await Task.Delay(100);

//    }

//    [Fact]
//    public async Task ExecuteAsync_ShouldNotThrow_WhenLoginFails()
//    {
//        // Arrange
//        _authService.LoginAsync(Arg.Any<LoginUserRequest>())
//                    .Returns(Task.FromResult<User>(null));

//        var service = CreateService();
//        SimulateConsoleInput("username", "password", "n");

//        // Act
//        await service.StartAsync(CancellationToken.None);
//        await Task.Delay(100);

//        // Assert: No exception expected
//    }

//    [Fact]
//    public async Task ExecuteAsync_ShouldCatchException()
//    {
//        // Arrange
//        _authService.LoginAsync(Arg.Any<LoginUserRequest>())
//                    .Throws(new Exception("Simulated exception"));

//        var service = CreateService();
//        SimulateConsoleInput("username", "password");

//        // Act
//        await service.StartAsync(CancellationToken.None);
//        await Task.Delay(100);

//        // Assert: Exception is caught → no crash
//    }
//}
