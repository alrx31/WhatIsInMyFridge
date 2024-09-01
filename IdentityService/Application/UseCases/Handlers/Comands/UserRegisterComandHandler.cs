using Application.Exceptions;
using Application.Services;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Infastructure.Persistanse;
using Infastructure.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Handlers.Comands
{
    internal class UserRegisterComandHandler(

        IUserRepository userRepository,
        IJWTService jwtService,
        ICacheRepository cacheRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) :IRequestHandler<UserRegisterCommand>
    {

        private readonly IUserRepository _repository = userRepository;
        private readonly IJWTService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheRepository _cacheRepository = cacheRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UserRegisterCommand model,CancellationToken cancellationToken)
        {
            var userCheck = await _repository.GetUserByLogin(model.Login);

            if (userCheck != null)
            {
                throw new AlreadyExistsException("This Login is not avaible");
            }

            await _repository.RegisterUser(new User
            {
                name = model.Name,
                login = model.Login,
                email = model.Email,
                password = Scripts.GetHash(model.Password),
                isAdmin = model.Name == "admin"
            });

            await _unitOfWork.CompleteAsync();

            var user = await _repository.GetUserByLogin(model.Login);

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

            await _repository.AddRefreshTokenField(newToken);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }

    }
}
