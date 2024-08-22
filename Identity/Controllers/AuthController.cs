using BuisnesLogic.Services;
using Microsoft.AspNetCore.Mvc;
using DataAcess.DTO;


namespace Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPut("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO model)
        {
            await _authService.RegisterUser(model);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO model)
        {
            var loginRes = await _authService.LoginUser(model);
            if (loginRes.IsLoggedIn) return Ok(loginRes);
            return Unauthorized();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            var loginRes = await _authService.RefreshToken(model);
            if (loginRes.IsLoggedIn) return Ok(loginRes);
            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutModel model)
        {
            await _authService.Logout(model);
            return Ok();
        }
    }
}
