using System;
using System.Threading.Tasks;

namespace Identity.Infrastructure
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}