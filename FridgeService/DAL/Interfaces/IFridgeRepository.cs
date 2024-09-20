using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IFridgeRepository
    {
        Task AddFridge(Fridge fridge);

        Task<Fridge?> GetFridge(int fridgeId);

        Task RemoveFridge(int fridgeId);

        Task<Fridge> UpdateFridge(Fridge fridge);

        Task AddUserToFridge(UserFridge model);

        Task RemoveUserFromFridge(int fridgeId, int userId);

        Task<List<int>> GetUsersFromFridge(int fridgeId);

        Task AddProductsToFridge(List<ProductFridgeModel> products);

        Task RemoveProductFromFridge(int fridgeId, string productId);

        Task<ProductFridgeModel> GetProductFromFridge(int fridgeId, string productId);

        Task<List<ProductFridgeModel>> GetProductsFromFridge(int fridgeId);

        Task<List<Fridge>> GetAllFridges();

        Task UpdateProductInFridge(ProductFridgeModel model);

        Task<List<Fridge>> GetFridgeByUserId(int userId);
        Task<Fridge> GetFridgeBySerial(string serial);
    }
}
