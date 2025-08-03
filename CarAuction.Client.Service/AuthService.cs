using AutoMapper;
using CarAuction.Client.Service.Interface;
using CarAuction.Common.Global.Enum;
using CarAuction.Dto;
using CarAuction.Dto.Request;
using CarAuction.Model;
using CarAuction.Sql.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Client.Service;
public class AuthService(IUserRepository userRepository, IAuctionRepository auctionRepository, IVehicleRepository vehicleRepository, IMapper mapper) : IAuthService
{
    public async Task<UserDto?> LoginAsync(LoginUserRequest request)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username);
        if (user == null) return null;

        if (user.PasswordHash != request.Password)
            return null;

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> RegisterAsync(RegisterUserRequest request)
    {
        var existing = await userRepository.GetByUsernameAsync(request.Username);
        if (existing != null)
            throw new InvalidOperationException("Username already exists");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = request.Password
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return mapper.Map<UserDto>(user);
    }
}

