using Application.DTO;
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
    public class UserLoginComandHandler(

        IUserRepository userRepository,
        IJWTService jwtService,
        ICacheRepository cacheRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        
        ): IRequestHandler<UserLoginCommand, LoginResponse>
    {

        private readonly IUserRepository _repository = userRepository;
        private readonly IJWTService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheRepository _cacheRepository = cacheRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<LoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var login = request.Login;
            var password = request.Password;
            var response = new LoginResponse();

            var identifyUser = await _repository.GetUserByLogin(login);

            if (identifyUser is null ||
                (identifyUser.password == Scripts.GetHash(password)) == false)
            {
                //return response;
                throw new BadRequestException("Invalid login or password");
            };

            response.IsLoggedIn = true;
            response.User = _mapper.Map<UserDTO>(identifyUser);
            response.JwtToken = _jwtService.GenerateJwtToken(identifyUser.email);
            var RefreshToken = _jwtService.GenerateRefreshToken();

            var identityUserTokenModel = await _repository.getTokenModel(identifyUser.email);

            if (identityUserTokenModel is null)
            {

                await _repository.AddRefreshTokenField(new RefreshTokenModel
                {
                    email = identifyUser.email,
                    refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12),
                    userId = identifyUser.id,
                    user = null
                });
            }
            else
            {
                identityUserTokenModel.refreshToken = RefreshToken;
                identityUserTokenModel.refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);
            }

            response.RefreshToken = RefreshToken;

            await _repository.UpdateRefreshTokenAsync(identityUserTokenModel);

            await _unitOfWork.CompleteAsync();

            await _cacheRepository.SetCatcheData($"user-{response.User.id}", response.User, new TimeSpan(24, 0, 0));

            return response;
        }
    }
}
