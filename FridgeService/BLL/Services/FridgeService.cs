using AutoMapper;
using DAL.Entities;
using BLL.DTO;
using BLL.Exceptions;
using DAL.Interfaces;
using DAL.IRepositories;
using DAL.Repositories;

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
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IgRPCService _grpcService;
        private readonly IProductsgRPCService _productsgRPCService;

        public FridgeService(
            IFridgeRepository fridgeRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IgRPCService igRPCService,
            IProductsgRPCService productsgRPCService
            )
        {
            _fridgeRepository = fridgeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _grpcService = igRPCService;
            _productsgRPCService = productsgRPCService;
        }

        public async Task AddFridge(FridgeAddDTO fridge)
        {
            await _unitOfWork.FridgeRepository.AddFridge(_mapper.Map<Fridge>(fridge));
         
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Fridge> GetFridge(int fridgeId)
        {
            var fridge = await _unitOfWork.FridgeRepository.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            return fridge;
        }

        public async Task RemoveFridgeById(int fridgeId)
        {
            await _unitOfWork.FridgeRepository.RemoveFridge(fridgeId);
        
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddUserToFridge(int fridgeId, int userId)
        {
            if(! await _grpcService.CheckUserExist(userId))
            {
                throw new NotFoundException("User not found");
            }

            await _unitOfWork.FridgeRepository.AddUserToFridge(fridgeId, userId);
        
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveUserFromFridge(int fridgeId, int userId)
        {
            if(fridgeId < 1 || userId < 1)
            {
                throw new BadRequestException("Invalid fridge or user id");
            }

            await _unitOfWork.FridgeRepository.RemoveUserFromFridge(fridgeId, userId);    

            await _unitOfWork.CompleteAsync();        
        }

        public async Task<Fridge> UpdateFridge(FridgeAddDTO fridge,int fridgeId)
        {
            var existingFridge = await _unitOfWork.FridgeRepository.GetFridge(fridgeId);
            
            if (existingFridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            _mapper.Map(fridge, existingFridge);

            await _unitOfWork.FridgeRepository.UpdateFridge(existingFridge);

            await _unitOfWork.CompleteAsync();

            return existingFridge;
        }

        public async Task AddProductsToList(int fridgeId, List<ProductInfoModel> products)
        {
            var fridge = await _unitOfWork.FridgeRepository.GetFridge(fridgeId);
            
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

            await _unitOfWork.FridgeRepository.AddProductsToFridge(productFridgeModels);
            
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveProductFromFridge(int fridgeId, string productId)
        {
            var fridge = await _unitOfWork.FridgeRepository.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            await _unitOfWork.FridgeRepository.RemoveProductFromFridge(fridgeId, productId);
            
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<User>> GetFridgeUsers(int fridgeId)
        {
            var fridge = await _unitOfWork.FridgeRepository.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var ids = await _unitOfWork.FridgeRepository.GetUsersFromFridge(fridgeId);

            return await _grpcService.GetUsers(ids);
        }

        public async Task CheckProducts(int fridgeId)
        {
            var fridge = await _unitOfWork.FridgeRepository.GetFridge(fridgeId);

            if(fridge is null)
        {
                throw new NotFoundException("Fridge not found");
            }

            var productsModel = await _unitOfWork.FridgeRepository.GetProductsFromFridge(fridgeId);

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
            var fridges = await _unitOfWork.FridgeRepository.GetAllFridges();

            foreach (var fridge in fridges)
            {
                await CheckProducts(fridge.id);
            }
            }

        public async Task<List<Product>> GetFridgeProducts(int fridgeId)
        {
            var productsModel = await _unitOfWork.FridgeRepository.GetProductsFromFridge(fridgeId);
               
            var ids = productsModel.Select(p => p.productId).ToList();

            return await _productsgRPCService.GetProducts(ids);
        }
    }
}
