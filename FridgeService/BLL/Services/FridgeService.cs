using AutoMapper;
using DAL.Entities;
using DAL.Repositories;
using BLL.DTO;
using Presentation.Middlewares.Exceptions;
using System.Net;
using DAL.IRepositories;
using AutoMapper.Configuration.Annotations;

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
            await _fridgeRepository.AddFridge(_mapper.Map<Fridge>(fridge));
         
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Fridge> GetFridge(int fridgeId)
        {
            var fridge =  await _fridgeRepository.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            return fridge;
        }

        public async Task RemoveFridgeById(int fridgeId)
        {
            await _fridgeRepository.RemoveFridge(fridgeId);
        
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddUserToFridge(int fridgeId, int userId)
        {
            if(! await _grpcService.CheckUserExist(userId))
            {
                throw new NotFoundException("User not found");
            }

            await _fridgeRepository.AddUserToFridge(fridgeId, userId);
        
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveUserFromFridge(int fridgeId, int userId)
        {
            if(fridgeId < 1 || userId < 1)
            {
                throw new BadRequestException("Invalid fridge or user id");
            }

            await _fridgeRepository.RemoveUserFromFridge(fridgeId, userId);

            await _unitOfWork.CompleteAsync();        
        }

        public async Task<Fridge> UpdateFridge(FridgeAddDTO fridge,int fridgeId)
        {
            var fridgeModel = await _fridgeRepository.GetFridge(fridgeId);
            
            if(fridgeModel == null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var updatedFridge = await _fridgeRepository
                .UpdateFridge(_mapper.Map<Fridge>((fridge, fridgeId)));

            await _unitOfWork.CompleteAsync();

            return updatedFridge;
        }

        public async Task AddProductsToList(int fridgeId, List<ProductInfoModel> products)
        {
            var fridge = await _fridgeRepository.GetFridge(fridgeId);
            
            if(fridge == null)
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

            await _fridgeRepository.AddProductsToFridge(productFridgeModels);
            
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveProductFromFridge(int fridgeId, string productId)
        {
            var fridge = await _fridgeRepository.GetFridge(fridgeId);
            
            if(fridge == null)
            {
                throw new NotFoundException("Fridge not found");
            }

            await _fridgeRepository.RemoveProductFromFridge(fridgeId, productId);
            
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<User>> GetFridgeUsers(int fridgeId)
        {
            var fridge = await _fridgeRepository.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var ids = await _fridgeRepository.GetUsersFromFridge(fridgeId);

            return await _grpcService.GetUsers(ids);
        }

        public async Task CheckProducts(int fridgeId)
        {
            var fridge = await _fridgeRepository.GetFridge(fridgeId);

            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var productsModel = await _fridgeRepository.GetProductsFromFridge(fridgeId);

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
            // check all products in all fridges
            var fridges = await _fridgeRepository.GetAllFridges();

            foreach (var fridge in fridges)
            {
                await CheckProducts(fridge.id);
            }
        }
    }
}
