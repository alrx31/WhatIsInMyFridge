
using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Comands;
using Domain.Entities;
using Domain.Repository;
using Npgsql.TypeMapping;
using System.Text;

namespace Application.Services
{
    public interface IUserService
    {
        Task DeleteUser(int id, int initiatorId);
        Task<User> getUserById(int id);
        Task<User> UpdateUser(UserRegisterCommand model, int id);
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

            if (user is null)
            {
                user = await _repository.getUserById(id);
            }

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var token = await _repository.getTokenModel(user.email);

            if (user.isAdmin || user.id == id)
            {

                await _repository.DeleteUser(id);
                
                if (token != null)
                {
                    await _repository.DeleteRefreshTokenByUserId(id);
                }

                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new BadRequestException("You do not have acess");
            }
        }

        public async Task<User> getUserById(int id)
        {
            var user1 = await _cacheRepository.GetCacheData<User>($"user-{id}");

            if (user1 != null)
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

        public async Task<User> UpdateUser(UserRegisterCommand model, int id)
        {
            var user = await _cacheRepository.GetCacheData<User>($"user-{id}");

            if (user is null)
            {
                user = await _repository.getUserById(id);
            }

            if (user is null)
            {
                throw new NotFoundException("User not Found");
            }

            user.login = model.Login;
            user.password = getHash(model.Password);
            user.email = model.Email;
            user.name = model.Name;

            User user1 = await _repository.UpdateUser(user, id);

            if (user1 is null)
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
