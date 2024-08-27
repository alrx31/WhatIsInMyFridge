using Application.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IFridgeService
    {
        Task AddFridge(FridgeAddDTO fridge);
        Task<Fridge> GetFridge(int fridgeId);
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
    }
}
