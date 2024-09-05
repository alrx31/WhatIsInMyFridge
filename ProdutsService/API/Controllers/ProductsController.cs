using Application.DTO;
using Application.UseCases.Comands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController:ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPut]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDTO product)
        {
            await _mediator.Send(_mapper.Map<AddProductComand>(product));
            return Ok();
        }
    }
}
