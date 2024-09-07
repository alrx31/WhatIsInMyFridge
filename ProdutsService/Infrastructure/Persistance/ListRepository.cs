using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance
{
    public class ListRepository : IListRepository
    {
        public Task AddList(ProductsList list)
        {
            throw new NotImplementedException();
        }

        public Task DeleteListById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductsList> GetListById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductsList> GetListByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(ProductsList ls)
        {
            throw new NotImplementedException();
        }
    }
}
