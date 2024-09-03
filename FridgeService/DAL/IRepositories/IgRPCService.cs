using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IgRPCService
    {
        Task<string> SayHello(string mess);
    }
}
