using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IFridgeRepository
    {
        Task AddFridge(Fridge fridge);

        Task<Fridge?> GetFridge(int fridgeId);

        Task RemoveFridge(int fridgeId);

        Task<Fridge> UpdateFridge(Fridge fridge,int fridgeId);

        Task AddUserToFridge(int fridgeId, int userId);

        Task RemoveUserFromFridge(int fridgeId, int userId);

        Task<List<User>> GetUsersFromFridge(int fridgeId);
    }
}
