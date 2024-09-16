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
    public class ListController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ListController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPut]
        public async Task<IActionResult> AddList([FromBody] AddListDTO model,CancellationToken cancellationToken)
        {
            await _mediator.Send(_mapper.Map<AddListComand>(model),cancellationToken);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetListById(string id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetListQuery>(id), cancellationToken));
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetListByName(string name, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetListByNameQuery>(name), cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListById(string id, CancellationToken cancellationToken)
        {
            await _mediator.Send(_mapper.Map<DeleteListComand>(id),cancellationToken);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateList(string id, [FromBody] AddListDTO model, CancellationToken cancellationToken)
        {
            await _mediator.Send((_mapper.Map<UpdateListComand>((model, id))), cancellationToken);

            return Ok();
        }

        // products in list

        [HttpPut("{listId}/product")]
        public async Task<IActionResult> AddProductToList(string listId, [FromBody] AddProductToListDTO model, CancellationToken cancellationToken)
        {
            await _mediator.Send(_mapper.Map<AddProductToListComand>((model, listId)),cancellationToken);

            return Ok();
        }

        [HttpGet("{listId}/products")]
        public async Task<IActionResult> GetListProducts(string listId, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetListProductsQuery>(listId),cancellationToken));
        }

        [HttpDelete("{listId}/{productId}")]
        public async Task<IActionResult> DeleteProductFromList(string listId, string productId, CancellationToken cancellationToken)
        {
            await _mediator.Send(_mapper.Map<DeleteProductInListComand>((listId, productId)), cancellationToken);

            return Ok();
        }
    }
}
