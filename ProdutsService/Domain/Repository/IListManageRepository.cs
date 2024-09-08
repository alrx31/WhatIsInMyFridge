using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IListManageRepository
    {
        Task AddProductToList(ProductInList productInList);

        Task DeleteProductInList(string listId,string productId);

        Task<List<string>> GetListProducts(string listId);
    }
}
