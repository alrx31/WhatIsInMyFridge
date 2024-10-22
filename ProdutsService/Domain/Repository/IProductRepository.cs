using Domain.Entities;

namespace Domain.Repository
{
    public interface IProductRepository:IBaseRepository<Product>
    {
        Task<Product> GetProductByName(string name, CancellationToken cancellationToken);

        Task<List<Product>> GetProductRange(List<string> listProductsModels,CancellationToken cancellationToken);

        Task<List<Product>> GetAllPaginationAsync(int page, int count,CancellationToken cancellationToken);
    }
}
