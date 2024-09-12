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
            await _mediator.Send((_mapper.Map<UpdateListComand>((model, id))));

            return Ok();
        }

        // products in list

        [HttpPut("{listId}/product")]
        public async Task<IActionResult> AddProductToList(string listId, [FromBody] AddProductToListDTO model)
        {
            await _mediator.Send(_mapper.Map<AddProductToListComand>((model, listId)));

            return Ok();
        }

        [HttpGet("{listId}/products")]
        public async Task<IActionResult> GetListProducts(string listId)
        {
            return Ok(await _mediator.Send(_mapper.Map<GetListProductsQuery>(listId)));
        }

        [HttpDelete("{listId}/{productId}")]
        public async Task<IActionResult> DeleteProductFromList(string listId, string productId)
        {
            await _mediator.Send(_mapper.Map<DeleteProductInListComand>((listId, productId)));

            return Ok();
        }
    }
}
