using Application.DTO;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecieptsController:ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RecieptsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [HttpPut]
        public async Task<IActionResult> AddReciept([FromBody] AddRecieptDTO model)
        {
            await _mediator.Send(_mapper.Map<AddRecieptComand>(model));
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecieptById(string id)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetRecieptQuery>(id)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReciept(string id)
        {
            await _mediator.Send(_mapper.Map<DeleteRecieptComand>(id));
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateReciept(string id, [FromBody] AddRecieptDTO model)
        {
            await _mediator.Send(_mapper.Map<UpdateRecieptComand>((model,id)));
            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReciepts([FromQuery] int page, [FromQuery] int count)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetAllRecieptsQuery>((page, count))));
        }

    }
}
