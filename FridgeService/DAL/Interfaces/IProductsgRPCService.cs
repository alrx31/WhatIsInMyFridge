namespace DAL.IRepositories
{
    public interface IProductsgRPCService
    {
        Task<List<DAL.Entities.Product>> GetProducts(List<string> ids);
    }
}
