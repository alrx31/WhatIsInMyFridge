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
            var user = await _unitOfWork.GetUserById(id);
            response.Users.Add(user)
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
    