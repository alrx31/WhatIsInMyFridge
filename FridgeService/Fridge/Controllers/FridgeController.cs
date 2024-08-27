using Application.DTO;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Infastructure.Middlewares.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FridgeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FridgeController:ControllerBase
    {
        private readonly IFridgeService _fridgeService;

        public FridgeController(IFridgeService fridgeService)
        {
            _fridgeService = fridgeService;
        }

        [HttpPut("add")]
        public async Task<IActionResult> AddFridge([FromBody] FridgeAddDTO fridge)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid fridge register model");
            }
            await _fridgeService.AddFridge(fridge);
            return Ok();
        }

        [HttpGet("{fridgeId}")]
        public async Task<IActionResult> getFridgeById(int fridgeId)
        { 
            return Ok(await _fridgeService.GetFridge(fridgeId));
        }


    }
}
