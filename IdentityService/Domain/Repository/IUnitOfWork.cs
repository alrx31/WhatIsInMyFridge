using System;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}