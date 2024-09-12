using AutoMapper;
using Domain.Repository;
using Grpc.Core;
using Infastructure.Persistanse.Protos;

public class GreeterService (
        IUnitOfWork unitOfWork,
        IMapper mapper
    ): Greeter.GreeterBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public override async Task<UsersResponse> GetUsers(UsersIds request, ServerCallContext context)
    {
        var response = new UsersResponse();

        
        foreach (var id in request.Ids)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(id);
            response.Users.Add(_mapper.Map<Infastructure.Persistanse.Protos.User>(user));
        }

        return response;
    }

    public override async Task<isUserExist> CheckUserExist(UserExistRequest request, ServerCallContext context)
    {
        var response = new isUserExist();
        
        response.IsExist = await _unitOfWork.UserRepository.GetUserById(request.UserId) != null;
        
        return response;
    }
}
    