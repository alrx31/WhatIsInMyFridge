using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IFridgeRepository
    {
        Task AddFridge(Fridge fridge);

        Task<Fridge?> GetFridge(int fridgeId);

        Task RemoveFridge(int fridgeId);

        Task<Fridge> UpdateFridge(Fridge fridge);

        Task AddUserToFridge(int fridgeId, int userId);

        Task RemoveUserFromFridge(int fridgeId, int userId);

        Task<List<int>> GetUsersFromFridge(int fridgeId);

        Task AddProductsToFridge(List<ProductFridgeModel> products);

        Task RemoveProductFromFridge(int fridgeId, string productId);

        Task<List<ProductFridgeModel>> GetProductsFromFridge(int fridgeId);

        Task<List<Fridge>> GetAllFridges();
    }
}
