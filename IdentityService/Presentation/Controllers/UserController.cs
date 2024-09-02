using Application.DTO;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController:ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _mediator.Send(_mapper.Map<GetUserQueryByIdQuery>(id));
            
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] int InitiatorId)
        {
            await _mediator.Send(_mapper.Map<DeleteUserCommand>((id, InitiatorId)));
            
            return Ok();
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] RegisterDTO model, int id)
        {
            var user = await _mediator.Send(_mapper.Map<UpdateUserCommand>((model, id)));

            return Ok(user);
        }

    }
}
