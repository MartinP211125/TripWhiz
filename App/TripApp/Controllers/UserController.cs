using Core.DTOs.Request;
using Core.DTOs.Request.User;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace TripApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // -------------------------
        // AUTHENTICATION
        // -------------------------

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginRequest)
        {
            string token = await userService.LoginAsync(loginRequest.Email, loginRequest.Password);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerRequest)
        {
            bool result = await userService.RegisterAsync(registerRequest);
            return Ok("Registration successful.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await userService.LogoutAsync();
            return Ok("Logout successful.");
        }

        // -------------------------
        // USER ACTIVITY
        // -------------------------

        [HttpPost("activity")]
        public async Task<IActionResult> CreateActivity([FromBody] UserActivitiesDto userActivity)
        {
            var result = await userService.CreateUserActivityForUserAsync(userActivity);
            return Ok(result);
        }
    }
}
