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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetProductQuery>(id)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _mediator.Send(_mapper.Map<DeleteProductComand>(id));
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] AddProductDTO product)
        {
            await _mediator.Send(_mapper.Map<UpdateProductComand>(product));
            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] int page, [FromQuery] int count)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetAllProductsQuery>((page, count))));
        }

    }
}
