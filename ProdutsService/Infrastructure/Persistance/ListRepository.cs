using Domain.Entities;
using Domain.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Infrastructure.Persistance
{
    public class ListRepository : IListRepository
    {
        private readonly IMongoCollection<ProductsList> _lists;

        public ListRepository(ApplicationDbContext context)
        {
            _lists = context.GetCollection<ProductsList>("Lists");
        }


        public async Task AddList(ProductsList list)
        {
            await _lists.InsertOneAsync(list);
        }

        public async Task DeleteListById(string id)
        {
            await _lists.DeleteOneAsync(l => l.Id == id);
        }

        public async Task<ProductsList> GetListById(string id)
        {
            return await _lists.Find(l => l.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ProductsList> GetListByName(string name)
        {
            return await _lists.Find(l => l.Name == name).FirstOrDefaultAsync();
        }

        public async Task UpdateList(ProductsList ls)
        {
            await _lists.ReplaceOneAsync(l => l.Id == ls.Id, ls);
        }
    }
}
