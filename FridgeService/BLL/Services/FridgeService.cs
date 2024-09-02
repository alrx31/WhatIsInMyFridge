using AutoMapper;
using DAL.Entities;
using DAL.Repositories;
using BLL.DTO;
using Presentation.Middlewares.Exceptions;

namespace BLL.Services
{
    public interface IFridgeService
    {
        Task AddFridge(FridgeAddDTO fridge);
        Task<Fridge> GetFridge(int fridgeId);
        Task RemoveFridgeById(int fridgeId);
        Task AddUserToFridge(int fridgeId, int userId);
        Task RemoveUserFromFridge(int fridgeId, int userId);
        Task<List<User>> GetUsersFromFridge(int fridgeId);
        Task<Fridge> UpdateFridge(FridgeAddDTO fridge,int fridgeId);
        Task AddProductsToList(int fridgeId, List<ProductInfoModel> products);
        Task RemoveProductFromFridge(int fridgeId, int productId);
    }

    public class FridgeService: IFridgeService
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public FridgeService(
            IFridgeRepository fridgeRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _fridgeRepository = fridgeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddFridge(FridgeAddDTO fridge)
        {

            await _fridgeRepository.AddFridge(_mapper.Map<Fridge>(fridge));
            await _unitOfWork.CompleteAsync();
        
        }

        public async Task<Fridge> GetFridge(int fridgeId)
        {
        
            var fridge =  await _fridgeRepository.GetFridge(fridgeId);
            
            if(fridge == null)
            {
                throw new DirectoryNotFoundException("Fridge not found");
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

        public async Task<List<User>> GetUsersFromFridge(int fridgeId)
        {
            if (await _fridgeRepository.GetFridge(fridgeId) == null)
            {
                throw new NotFoundException("Fridge not found");
            }
            
            return await _fridgeRepository.GetUsersFromFridge(fridgeId);
        }

        public async Task<Fridge> UpdateFridge(FridgeAddDTO fridge,int fridgeId)
        {
            var fridgeModel = await _fridgeRepository.GetFridge(fridgeId);
            
            if(fridgeModel == null)
            {
                throw new NotFoundException("Fridge not found");
            }

            return await _fridgeRepository
                .UpdateFridge(_mapper.Map<Fridge>(fridge),fridgeId);
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
                    count = product.count,
                    addTime = DateTime.UtcNow
                });
            }
            await _fridgeRepository.AddProductsToFridge(productFridgeModels);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveProductFromFridge(int fridgeId, int productId)
        {
            var fridge = await _fridgeRepository.GetFridge(fridgeId);
            
            if(fridge == null)
            {
                throw new NotFoundException("Fridge not found");
            }

            await _fridgeRepository.RemoveProductFromFridge(fridgeId, productId);
            await _unitOfWork.CompleteAsync();
        }
    }
}
