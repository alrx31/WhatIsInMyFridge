using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListController:ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ListController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPut]
        public async Task<IActionResult> AddList([FromBody] AddListDTO model)
        {
            await _mediator.Send(_mapper.Map<AddListComand>(model));

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetListById(string id)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetListQuery>(id)));
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetListByName(string name)
        {
            await _mediator.Send(_mapper.Map<GetListByNameQuery>(name));
            
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListById(string id)
        {
            await _mediator.Send(_mapper.Map<DeleteListComand>(id));
         
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateList(string id, [FromBody] AddListDTO model)
        {
            await _mediator.Send((_mapper.Map<UpdateListComand>((model,id))));
        
            return Ok();
        }
    }
}
