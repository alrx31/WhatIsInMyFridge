using Application.DTO;
using Application.Services;
using Application.UseCases.Comands;
using AutoMapper;
using MediatR;
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
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(
            IAuthService authService, 
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator,
            IMapper mapper
            )
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPut("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO model)
        {
            await _mediator.Send(_mapper.Map<UserRegisterCommand>(model));
            
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO model)
        {
            //var loginRes = await _authService.LoginUser(model);
            var loginRes = await _mediator.Send(_mapper.Map<UserLoginCommand>(model));

            SetRefreshTokenCookie(loginRes.RefreshToken);

            return Ok(loginRes);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            var loginRes = await _mediator.Send(_mapper.Map<RefreshTokenCommand>(model));

            SetRefreshTokenCookie(loginRes.RefreshToken);

            return Ok(loginRes);
        }

        [HttpPost("logout/{UserId}")]
        public async Task<IActionResult> Logout(int UserId)
        {
            await _mediator.Send(_mapper.Map<UserLogoutCommand>(UserId));

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
