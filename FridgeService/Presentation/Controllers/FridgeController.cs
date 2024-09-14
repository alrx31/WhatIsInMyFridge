using BLL.DTO;
using BLL.Services;
using DAL.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares.Exceptions;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FridgeController:ControllerBase
    {
        private readonly IFridgeService _fridgeService;
        private readonly IgRPCService _gRPCService;

        public FridgeController(
            IFridgeService fridgeService
            )
        {
            _fridgeService = fridgeService;
            _gRPCService = gRPCService;
        }

        [HttpPut]
        public async Task<IActionResult> AddFridge([FromBody] FridgeAddDTO fridge)
        { 
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

        [HttpPut("{fridgeId}/users/{userId}")]
        public async Task<IActionResult> AddUserToFridge(int fridgeId, int userId)
        {
            await _fridgeService.AddUserToFridge(fridgeId, userId);
            
            return Ok();
        }

        [HttpDelete("{fridgeId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromFridge(int fridgeId, int userId)
        {
            await _fridgeService.RemoveUserFromFridge(fridgeId, userId);
            
            return Ok();
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

        [HttpPut("{fridgeId}/products")]
        public async Task<IActionResult> AddProductsToFridge([FromBody] List<ProductInfoModel> products, int fridgeId)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid products model");
            }

            await _fridgeService.AddProductsToList(fridgeId, products);
            
            return Ok();
        }

        [HttpDelete("{fridgeId}/products/{productId}")]
        public async Task<IActionResult> RemoveProductFromFridge(int fridgeId, string productId)
        {
            await _fridgeService.RemoveProductFromFridge(fridgeId, productId);
            
            return Ok();
        }

        [HttpGet("{fridgeId}/users")]
        public async Task<IActionResult> GetUsers(int fridgeId)
        {
            return Ok(await _fridgeService.GetFridgeUsers(fridgeId));
        }

        [HttpPost("{fridgeId}/check-products")]
        public async Task<IActionResult> CheckProducts(int fridgeId)
        {
            await _fridgeService.CheckProducts(fridgeId);

            return Ok();
        }

        [HttpGet("{fridgeId}/products")]
        public async Task<IActionResult> GetProducts(int fridgeId)
        {
            return Ok(await _fridgeService.GetFridgeProducts(fridgeId));
        }
    }
}
