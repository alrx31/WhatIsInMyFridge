﻿using Application.DTO;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator,
            IMapper mapper
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPut("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterCommand model)
        {
            await _mediator.Send(model);
            
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginCommand model)
        {
            var loginRes = await _mediator.Send(model);

            SetRefreshTokenCookie(loginRes.RefreshToken);

            return Ok(loginRes);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand model)
        {
            var loginRes = await _mediator.Send(model);

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
