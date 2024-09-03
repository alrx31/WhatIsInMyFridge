using DAL.IRepositories;
using DAL.Persistanse.Protos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistanse
{
    public class gRPCService: IgRPCService
    {
        private readonly Greeter.GreeterClient _greeterClient;

        public gRPCService(Greeter.GreeterClient greeterClient)
        {
            _greeterClient = greeterClient;
        }

        public async Task<string> SayHello(string name)
        {
            var reply = await _greeterClient.SayHelloAsync(
                                             new HelloRequest { Name = name });
            return reply.Message;
        }
    }

}
