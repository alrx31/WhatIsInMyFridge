using Application.DTO;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.getUserById(id);
            
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] int InitiatorId)
        {
            await _userService.DeleteUser(id,InitiatorId);
            
            return Ok();
        }
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] RegisterDTO model, int id)
        {
            var user = await _userService.UpdateUser(model, id);

            return Ok(user);
        }

    }
}
