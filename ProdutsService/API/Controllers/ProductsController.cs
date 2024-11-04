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
        public async Task<IActionResult> AddProduct([FromBody] AddProductDTO product,CancellationToken cancellationToken)
        {
            await _mediator.Send(_mapper.Map<AddProductComand>(product),cancellationToken);
           
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetProductQuery>(id), cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id, CancellationToken cancellationToken)
        {
            await _mediator.Send(_mapper.Map<DeleteProductComand>(id),cancellationToken);
            
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] AddProductDTO product, CancellationToken cancellationToken)
        {
            await _mediator.Send(_mapper.Map<UpdateProductComand>((product,id)),cancellationToken);
            
            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] int page, [FromQuery] int count, CancellationToken cancellationToken)
        {
            var prs = await _mediator.Send(_mapper.Map<GetAllProductsQuery>((page, count)), cancellationToken);
            return Ok(prs);
        }
    }
}
