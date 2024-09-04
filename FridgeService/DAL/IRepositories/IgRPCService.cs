using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IgRPCService
    {
        Task<List<User>> GetUsers(List<int> ids);
        Task<bool> CheckUserExist(int userId);
    }
}
