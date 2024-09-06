using Domain.Repository;
using Grpc.Core;
using Infastructure.Persistanse.Protos;

public class GreeterService (
        IUnitOfWork unitOfWork
    ): Greeter.GreeterBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public override Task<UsersResponse> GetUsers(UsersIds request, ServerCallContext context)
    {
        var response = new UsersResponse();

        
        foreach (var id in request.Ids)
        {
            var user = _unitOfWork.GetUserById(id);
            response.Users.Add(new User
            {
                Id = user.Result.id,
                Name = user.Result.name,
                Login = user.Result.login,
                Email = user.Result.email,
                IsAdmin = user.Result.isAdmin,
                Password = user.Result.password
            });
        }

        return Task.FromResult(response);
    }

    public override async Task<isUserExist> CheckUserExist(UserExistRequest request, ServerCallContext context)
    {
        var response = new isUserExist();
        
        response.IsExist = await _unitOfWork.GetUserById(request.UserId) != null;
        
        return response;
    }
}
    