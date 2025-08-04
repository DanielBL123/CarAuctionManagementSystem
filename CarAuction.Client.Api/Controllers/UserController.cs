using CarAuction.Client.Service.Interface;
using CarAuction.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarAuction.Client.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class UserController(IAuthService authService, IBidService bidService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            try
            {
                var user = await authService.LoginAsync(request);
                if (user == null)
                    return NotFound(new { message = "User not found or password incorrect" });

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("MyDevelopmentSecretKeyCodingTestAuctionClientSide12345");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                    [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
            ]),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(new { token = jwtToken });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            try
            {
                var newUser = await authService.RegisterAsync(request);
                return Ok(newUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("bid")]
        public async Task<IActionResult> PlaceBid([FromBody] CreateBidRequest createBidRequest)
        {
            try
            {
                var username = User.FindFirstValue(ClaimTypes.Name)!;
                await bidService.PlaceBid(createBidRequest, username);
                return Ok("Bid placed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
        [HttpGet("vehicles")]
        public async Task<IActionResult> GetVehiclesFromUser()
        {
            try
            {
                var username = User.FindFirstValue(ClaimTypes.Name)!;
                return Ok(await authService.GetVehicles(username));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
