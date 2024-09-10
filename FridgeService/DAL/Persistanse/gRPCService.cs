using DAL.IRepositories;
using DAL.Persistanse.Protos;

namespace DAL.Persistanse
{
    public class gRPCService : IgRPCService
    {
        private readonly Greeter.GreeterClient _greeterClient;

        public gRPCService(Greeter.GreeterClient greeterClient)
        {
            _greeterClient = greeterClient;
        }

        public async Task<List<DAL.Entities.User>> GetUsers(List<int> ids)
        {
            var UsersIds = new UsersIds();

            UsersIds.Ids.AddRange(ids);
            
            var reply = await _greeterClient.GetUsersAsync(UsersIds);
            
            return reply.Users.Select(u => new DAL.Entities.User
            {
                id = u.Id,
                name = u.Name,
                login = u.Login,
                email = u.Email,
                password = u.Password,
                isAdmin = u.IsAdmin

            }).ToList<DAL.Entities.User>(); ;

        }
        public async Task<bool> CheckUserExist(int userid)
        {
            var res = new UserExistRequest();
            res.UserId = userid;

            var reply = await _greeterClient.CheckUserExistAsync(res);
            
            return reply.IsExist;
        }
    }

}
