using CarAuction.Dto;
using CarAuction.Dto.Request;

namespace CarAuction.Client.Service.Interface;

public interface IAuthService
{
    Task<UserDto?> LoginAsync(LoginUserRequest request);
    Task<UserDto> RegisterAsync(RegisterUserRequest request);
}
