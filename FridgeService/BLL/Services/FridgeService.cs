using AutoMapper;
using DAL.Entities;
using DAL.Repositories;
using BLL.DTO;
using DAL.IRepositories;
using BLL.Exceptions;

namespace BLL.Services
{
    public interface IFridgeService
    {
        Task AddFridge(FridgeAddDTO fridge);

        Task<Fridge> GetFridge(int fridgeId);
        
        Task RemoveFridgeById(int fridgeId);
        
        Task AddUserToFridge(int fridgeId, int userId);
        
        Task RemoveUserFromFridge(int fridgeId, int userId);
        
        Task<Fridge> UpdateFridge(FridgeAddDTO fridge,int fridgeId);
        
        Task AddProductsToList(int fridgeId, List<ProductInfoModel> products);
        
        Task RemoveProductFromFridge(int fridgeId, string productId);
        
        Task<List<User>> GetFridgeUsers(int fridgeId);
        
        Task CheckProducts(int fridgeId);
        
        Task CheckProducts();
        
        Task<List<Product>> GetFridgeProducts(int fridgeId);
    }

    public class FridgeService: IFridgeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IgRPCService _grpcService;
        private readonly IProductsgRPCService _productsgRPCService;

        public FridgeService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IgRPCService igRPCService,
            IProductsgRPCService productsgRPCService
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _grpcService = igRPCService;
            _productsgRPCService = productsgRPCService;
        }

        public async Task AddFridge(FridgeAddDTO fridge)
        {
            await _unitOfWork.AddFridge(_mapper.Map<Fridge>(fridge));
        }

        public async Task<Fridge> GetFridge(int fridgeId)
        {
            var fridge =  await _unitOfWork.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            return fridge;
        }

        public async Task RemoveFridgeById(int fridgeId)
        {
            await _unitOfWork.RemoveFridge(fridgeId);
        }

        public async Task AddUserToFridge(int fridgeId, int userId)
        {
            if(! await _grpcService.CheckUserExist(userId))
            {
                throw new NotFoundException("User not found");
            }

            await _unitOfWork.AddUserToFridge(fridgeId, userId);
        }

        public async Task RemoveUserFromFridge(int fridgeId, int userId)
        {
            if(fridgeId < 1 || userId < 1)
            {
                throw new BadRequestException("Invalid fridge or user id");
            }

            await _unitOfWork.RemoveUserFromFridge(fridgeId, userId);    
        }

        public async Task<Fridge> UpdateFridge(FridgeAddDTO fridge,int fridgeId)
        {
            var existingFridge = await _unitOfWork.GetFridge(fridgeId);

            if (existingFridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            _mapper.Map(fridge, existingFridge);

            await _unitOfWork.UpdateFridge(existingFridge);

            return existingFridge;
        }

        public async Task AddProductsToList(int fridgeId, List<ProductInfoModel> products)
        {
            var fridge = await _unitOfWork.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            List<ProductFridgeModel> productFridgeModels = new List<ProductFridgeModel>();

            foreach (var product in products)
            {
                productFridgeModels.Add(new ProductFridgeModel
                {
                    fridgeId = fridgeId,
                    productId = product.ProductId,
                    count = product.Count,
                    addTime = DateTime.UtcNow
                });
            }

            await _unitOfWork.AddProductsToFridge(productFridgeModels);
        }

        public async Task RemoveProductFromFridge(int fridgeId, string productId)
        {
            var fridge = await _unitOfWork.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            await _unitOfWork.RemoveProductFromFridge(fridgeId, productId);
        }

        public async Task<List<User>> GetFridgeUsers(int fridgeId)
        {
            var fridge = await _unitOfWork.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var ids = await _unitOfWork.GetUsersFromFridge(fridgeId);

            return await _grpcService.GetUsers(ids);
        }

        public async Task CheckProducts(int fridgeId)
        {
            var fridge = await _unitOfWork.GetFridge(fridgeId);

            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var productsModel = await _unitOfWork.GetProductsFromFridge(fridgeId);

            var ids = productsModel.Select(p => p.productId).ToList();

            var products = await _productsgRPCService.GetProducts(ids);

            for(var i = 0; i < products.Count; i++ )
            {
                if (productsModel[i].addTime + products[i].ExpirationTime < DateTime.UtcNow)
                {
                    // Use SignalR to notify user
                }
            }
        }

        public async Task CheckProducts()
        {
            var fridges = await _unitOfWork.GetAllFridges();

            foreach (var fridge in fridges)
            {
                await CheckProducts(fridge.id);
            }
        }

        public async Task<List<Product>> GetFridgeProducts(int fridgeId)
        {
            var productsModel = await _unitOfWork.GetProductsFromFridge(fridgeId);
               
            var ids = productsModel.Select(p => p.productId).ToList();

            return await _productsgRPCService.GetProducts(ids);
        }
    }
}
