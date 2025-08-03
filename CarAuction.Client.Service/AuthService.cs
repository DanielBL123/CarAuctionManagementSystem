using AutoMapper;
using CarAuction.Client.Service.Interface;
using CarAuction.Common.Global.Enum;
using CarAuction.Dto;
using CarAuction.Dto.Request;
using CarAuction.Model;
using CarAuction.Sql.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Client.Service;
public class AuthService(IUserRepository userRepository, IVehicleRepository vehicleRepository, IMapper mapper) : IAuthService
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

    public async Task<IEnumerable<VehicleDto>> GetVehicles(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        ArgumentNullException.ThrowIfNull(user);

        return await Task.Run(() => mapper.Map<IEnumerable<VehicleDto>>(vehicleRepository.AsQueryable(x => x.UserId == user.Id)));
    }

}

