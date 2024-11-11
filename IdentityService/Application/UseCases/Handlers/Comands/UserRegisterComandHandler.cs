using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Infastructure.Persistanse;
using Infastructure.Services;
using MediatR;

namespace Application.UseCases.Handlers.Comands
{
    public class UserRegisterComandHandler(

        IJWTService jwtService,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) :IRequestHandler<UserRegisterCommand>
    {

        private readonly IJWTService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UserRegisterCommand model,CancellationToken cancellationToken)
        {
            var userCheck = await _unitOfWork.UserRepository.GetUserByLogin(model.Login);

            if (!(userCheck is null))
            {
                throw new AlreadyExistsException("This Login is not avaible");
            }

            model.Password = Scripts.GetHash(model.Password);   

            var userModel = _mapper.Map<User>(model);

            if(userModel.login == "admin")
            {
                userModel.isAdmin = true;
            }

            await _unitOfWork.UserRepository.RegisterUser(userModel);

            await _unitOfWork.CompleteAsync();

            var user = await _unitOfWork.UserRepository.GetUserByLogin(model.Login);

            if (user is null)
            {
                throw new ValidationDataException("User not found");
            }

            var newToken = new RefreshTokenModel
            {
                email = model.Email,
                refreshToken = "",
                refreshTokenExpiryTime = DateTime.UtcNow,
                userId = user.id,
                user = null
            };

            await _unitOfWork.UserRepository.AddRefreshTokenField(newToken);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }

    }
}
