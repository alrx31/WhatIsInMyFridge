
using Application.DTO;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Components.Forms;
using System.Text;

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

        public UserService(IUserRepository repository,IUnitOfWork unitOfWork )
        {

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteUser(int id, int initiatorId)
        {
            var user = await _repository.getUserById(initiatorId);
            
            if(user is null)
            {
                throw new Exception("User not found");
            }

            if (user.isAdmin)
            {
                await _repository.DeleteUser(id);
                
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new Exception("You are not an admin");
            }
        }

        public async Task<User> getUserById(int id)
        {

            return await _repository.getUserById(id) ?? throw new Exception("User not found");
        
        }

        public async Task<User> UpdateUser(RegisterDTO model, int id)
        {

            var user = await _repository.getUserById(id);

            if (user is null)
            {
                throw new Exception("User not Found");
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
                throw new Exception("Invalid User");
            }

            await _unitOfWork.CompleteAsync();

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
