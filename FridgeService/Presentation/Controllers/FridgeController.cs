﻿using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares.Exceptions;

namespace Presentation.Controllers
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
        public async Task<IActionResult> GetFridgeById(int fridgeId)
        { 
            
            return Ok(await _fridgeService.GetFridge(fridgeId));
        }

        [HttpDelete("{fridgeId}")]
        public async Task<IActionResult> RemoveFridgeById(int fridgeId)
        {
            
            await _fridgeService.RemoveFridgeById(fridgeId);
         
            return Ok();
        }

        [HttpPut("{fridgeId}/addUser/{userId}")]
        public async Task<IActionResult> AddUserToFridge(int fridgeId, int userId)
        {
            
            await _fridgeService.AddUserToFridge(fridgeId, userId);
            
            return Ok();
        }

        [HttpDelete("{fridgeId}/removeUser/{userId}")]
        public async Task<IActionResult> RemoveUserFromFridge(int fridgeId, int userId)
        {
            
            await _fridgeService.RemoveUserFromFridge(fridgeId, userId);
            
            return Ok();
        }

        [HttpGet("{fridgeId}/users")]
        public async Task<IActionResult> GetUsersFromFridge(int fridgeId)
        {

            return Ok(await _fridgeService.GetUsersFromFridge(fridgeId));
        }

        [HttpPatch("{fridgeId}")]
        public async Task<IActionResult> UpdateFridge([FromBody] FridgeAddDTO fridge, int fridgeId)
        {

            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid fridge model");
            }

            var res = await _fridgeService.UpdateFridge(fridge, fridgeId);
            
            return Ok(res);
        }

        [HttpPut("{fridgeId}/addProducts")]
        public async Task<IActionResult> AddProductsToFridge([FromBody] List<ProductInfoModel> products, int fridgeId)
        {

            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid products model");
            }

            await _fridgeService.AddProductsToList(fridgeId, products);
            
            return Ok();
        }

        [HttpDelete("{fridgeId}/removeProduct/{productId}")]
        public async Task<IActionResult> RemoveProductFromFridge(int fridgeId, int productId)
        {
            
            await _fridgeService.RemoveProductFromFridge(fridgeId, productId);
            
            return Ok();
        }
    }
}
