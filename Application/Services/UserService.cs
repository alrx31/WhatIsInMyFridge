
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
    public interface IUserService
    {
        Task DeleteUser(int id, int initiatorId);
        Task<User> getUserById(int id);
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
    }
}
