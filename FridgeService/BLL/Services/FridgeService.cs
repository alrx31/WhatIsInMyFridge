﻿using AutoMapper;
using DAL.Entities;
using BLL.DTO;
using BLL.Exceptions;
using DAL.Interfaces;
using DAL.IRepositories;
using DAL.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DAL.Persistanse.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;

namespace BLL.Services
{
    public interface IFridgeService
    {
        Task AddFridge(FridgeAddDTO fridge);

        Task<Fridge> GetFridge(int fridgeId);
        
        Task RemoveFridgeById(int fridgeId);
        
        Task AddUserToFridge(string serial,int boxNumber, int userId);
        
        Task RemoveUserFromFridge(int fridgeId, int userId);
        
        Task<Fridge> UpdateFridge(FridgeAddDTO fridge,int fridgeId);
        
        Task AddProductsToFridge(int fridgeId, List<ProductInfoModel> products);
        
        Task RemoveProductFromFridge(int fridgeId, string productId);
        
        Task<List<User>> GetFridgeUsers(int fridgeId);
        
        Task CheckProducts(int fridgeId);
        
        Task CheckProducts();
        
        Task<List<Product>> GetFridgeProducts(int fridgeId);

        Task DevideProductFromFridge(int fridgeId, int count, string productId);

        Task<List<Fridge>> GetFridgesByUserId(int userId);
        
        Task<Fridge> GetFridgeBySerialAndBoxNumber(string serial,int boxNumber);
    }

    public class FridgeService : IFridgeService
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IgRPCService _grpcService;
        private readonly IProductsgRPCService _productsgRPCService;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<FridgeService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public FridgeService(
            IFridgeRepository fridgeRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IgRPCService igRPCService,
            IProductsgRPCService productsgRPCService,
            IKafkaProducer kafkaProducer,
            ILogger<FridgeService> logger,
            IHubContext<NotificationHub> hubContext
            )
        {
            _fridgeRepository = fridgeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _grpcService = igRPCService;
            _productsgRPCService = productsgRPCService;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
            _hubContext = hubContext;
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

        public async Task<List<Fridge>> GetFridgesByUserId(int userId)
        {
            var isUserExist = await _grpcService.CheckUserExist(userId);

            if (!isUserExist)
            {
                throw new NotFoundException("User not found");
            }

            var fridge = await _unitOfWork.FridgeRepository.GetFridgeByUserId(userId);

            if (fridge is null)
            {
                throw new NotFoundException("Fridges not found");
            }

            return fridge;
        }

        public async Task RemoveFridgeById(int fridgeId)
        {
            await _unitOfWork.FridgeRepository.RemoveFridge(fridgeId);
        
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddUserToFridge(string serial,int boxNumber, int userId)
        {
            if(! await _grpcService.CheckUserExist(userId))
            {
                throw new NotFoundException("User not found");
            }
            var fridge = await _unitOfWork.FridgeRepository.GetFridgeBySerialAndBoxNumber(serial,boxNumber);

            if (fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var modelT = await _unitOfWork.FridgeRepository.GetUsersFromFridge(fridge.id);

            if (modelT.Contains(userId))
            {
                throw new BadRequestException("User already use this fridge");
            }


            var model = new UserFridge
            {
                userId = userId,
                fridgeId = fridge.id,
                LinkTime = DateTime.UtcNow
            };

            await _unitOfWork.FridgeRepository.AddUserToFridge(model);
        
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

        public async Task AddProductsToFridge(int fridgeId, List<ProductInfoModel> products)
        {
            var fridge = await _unitOfWork.FridgeRepository.GetFridge(fridgeId);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }



            List<ProductFridgeModel> productFridgeModels = new List<ProductFridgeModel>();
            List<ProductFridgeModel> productFridgeModelsToKafka = new List<ProductFridgeModel>();


            foreach (var product in products)
            {
                var model = await _unitOfWork.FridgeRepository.GetProductFromFridge(fridgeId, product.ProductId);
                
                if (model is not null)
                {
                    _logger.LogInformation("product in fridge exist");
                    _logger.LogInformation($"count before: {model.count}");
                    model.count += product.Count;
                    _logger.LogInformation($"count after: {model.count}");
                    
                    await _unitOfWork.FridgeRepository.UpdateProductInFridge(model);
                    
                    await _unitOfWork.CompleteAsync();
                    
                    model.count = product.Count;
                    productFridgeModelsToKafka.Add(model);
                    continue;
                }
                else
                {
                    _logger.LogInformation("product in fridge not exist");
                    productFridgeModels.Add(new ProductFridgeModel
                    {
                        fridgeId = fridgeId,
                        productId = product.ProductId,
                        count = product.Count,
                        addTime = DateTime.UtcNow
                    });
                }
            }

            _logger.LogInformation("update products in fridge");

            if (productFridgeModels.Any())
            {
                _logger.LogInformation("Adding new products to fridge");
                await _unitOfWork.FridgeRepository.AddProductsToFridge(productFridgeModels);
                await _unitOfWork.CompleteAsync();
            }

            _logger.LogInformation("send message to kafka");

            var message = new DAL.Entities.MessageBrokerEntities.Product
            (
                _mapper.Map<List<DAL.Entities.MessageBrokerEntities.ProductInfo>>(productFridgeModelsToKafka),
                fridgeId
            );

            await _kafkaProducer.ProduceAsync<DAL.Entities.MessageBrokerEntities.Product>("AddProducts", message);
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

            var message = _mapper.Map<DAL.Entities.MessageBrokerEntities.ProductRemove>((productId,fridgeId));

            await _kafkaProducer.ProduceAsync<DAL.Entities.MessageBrokerEntities.ProductRemove>("RemoveProduct",message);
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

            if (fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var productsModel = await _unitOfWork.FridgeRepository.GetProductsFromFridge(fridgeId);
            
            var ids = productsModel.Select(p => p.productId).ToList();
            
            var products = await _productsgRPCService.GetProducts(ids);

            var users = await GetFridgeUsers(fridgeId);

            for (var i = 0; i < products.Count; i++)
            {
                if (productsModel[i].addTime + products[i].ExpirationTime < DateTime.UtcNow.AddDays(3))
                {
                    foreach (var user in users)
                    {
                        await _hubContext.Clients.Group(fridgeId.ToString())
                           .SendAsync("ReceiveNotification", $"{products[i].Name} in Fridge: {fridge.name}");
                    }
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

            var PR = await _productsgRPCService.GetProducts(ids);
            
            for(var i = 0; i < PR.Count;i++)
            {
                PR[i].Count = productsModel[i].count;
                PR[i].AddTime = productsModel[i].addTime;
            }

            return PR;
        }

        public async Task DevideProductFromFridge(int fridgeId, int count, string productId)
        {
            if(count == 0)
            {
                await this.RemoveProductFromFridge(fridgeId, productId);

                return;
            }

            var fridge = await _unitOfWork.FridgeRepository.GetFridge(fridgeId);

            if (fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            var model = await _unitOfWork.FridgeRepository.GetProductFromFridge(fridgeId, productId);

            if (model is null)
            {
                throw new NotFoundException("Product in fridge not found");
            }

            model.count = model.count - count;
            
            if (model.count < 0)
            {
                throw new BadRequestException("Invalid count of product");
            }

            if (model.count == 0)
            {
                await _unitOfWork.FridgeRepository.RemoveProductFromFridge(fridgeId, productId);
            }
            else
            {
                await _unitOfWork.FridgeRepository.UpdateProductInFridge(model);
            }

            await _unitOfWork.CompleteAsync();

            var message = _mapper.Map<DAL.Entities.MessageBrokerEntities.ProductRemove>((productId, fridgeId,count));

            await _kafkaProducer.ProduceAsync<DAL.Entities.MessageBrokerEntities.ProductRemove>("RemoveProduct", message);
        }

        public async Task<Fridge> GetFridgeBySerialAndBoxNumber(string serial,int boxNumber)
        {
            var fridge = await _unitOfWork.FridgeRepository.GetFridgeBySerialAndBoxNumber(serial,boxNumber);
            
            if(fridge is null)
            {
                throw new NotFoundException("Fridge not found");
            }

            return fridge;
        }
    }
}
