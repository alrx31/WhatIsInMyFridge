using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
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

            return loginRes.IsLoggedIn ? Ok(loginRes) : Unauthorized();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            var loginRes = await _authService.RefreshToken(model);

            return loginRes.IsLoggedIn ? Ok(loginRes) : Unauthorized();
        }

        //TODO
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDTO model)
        {
            await _authService.Logout(model);
            
            return Ok();
        }
    }
}
