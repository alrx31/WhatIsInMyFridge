
using Application.DTO;
using Domain.Entities;
using Domain.Repository;
using Infastructure.Middlewares.Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public interface IUserService
    {
        Task DeleteUser(int id, int initiatorId);
        Task<User> getUserById(int id);
        Task<User> UpdateUser(RegisterDTO model, int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheRepository _cacheRepository;

        public UserService(
            IUserRepository repository,
            IUnitOfWork unitOfWork,
            ICacheRepository cacheRepository
        )
        {

            _repository = repository;
            _unitOfWork = unitOfWork;
            _cacheRepository = cacheRepository;
        }

        public async Task DeleteUser(int id, int initiatorId)
        {
            var user = await _cacheRepository.GetCacheData<User>($"user-{initiatorId}");
            
            if (user == null)
            {
                user = await _repository.getUserById(id);
            }

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (user.isAdmin)
            {

                await _repository.DeleteUser(id);
                
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new ForbiddenException("You do not have acess");
            }
        }

        public async Task<User> getUserById(int id)
        {

            var user1 = await _cacheRepository.GetCacheData<User>($"user-{id}");
            
            if(user1 != null)
            {
                return user1;
            }
            
            var user = await _repository.getUserById(id) ?? throw new NotFoundException("User not found");
            
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            return user;
        }

        public async Task<User> UpdateUser(RegisterDTO model, int id)
        {

            var user = await _cacheRepository.GetCacheData<User>($"user-{id}");
            
            if (user == null)
            {
                user = await _repository.getUserById(id);
            }

            if (user == null)
            {
                throw new NotFoundException("User not Found");
            }

            if(!string.IsNullOrEmpty(user.login))
            {
                user.login = model.login;
            }

            if(!string.IsNullOrEmpty(user.password))
            {
                user.password = getHash(model.password);
            }

            if(!string.IsNullOrEmpty(user.email))
            {
                user.email = model.email;
            }

            User user1 = await _repository.UpdateUser(user, id);
            
            if(user1 is null)
            {
                throw new NotFoundException("Invalid User");
            }

            await _unitOfWork.CompleteAsync();

            await _cacheRepository.SetCatcheData($"user-{id}", user1);

            return user1;
        }

        private string getHash(string pass)
        {

            var data = System.Text.Encoding.ASCII.GetBytes(pass);
            data = System.Security.Cryptography.SHA256.HashData(data);

            return Encoding.ASCII.GetString(data);
        }
    }
}
