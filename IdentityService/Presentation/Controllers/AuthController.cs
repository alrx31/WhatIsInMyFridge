using Application.DTO;
using Application.Services;
using Infastructure.Middlewares.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
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

            SetRefreshTokenCookie(loginRes.RefreshToken);

            return Ok(loginRes);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            var loginRes = await _authService.RefreshToken(model);

            SetRefreshTokenCookie(loginRes.RefreshToken);

            return Ok(loginRes);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDTO model)
        {
            await _authService.Logout(model);

            ClearRefreshTokenCookie();

            return Ok();
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        private void ClearRefreshTokenCookie()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
        }

    }
}
