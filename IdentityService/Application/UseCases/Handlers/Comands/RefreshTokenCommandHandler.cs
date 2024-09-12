using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Infastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Handlers.Comands
{
    public class RefreshTokenCommandHandler(

        IJWTService jwtService,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper

        ) :IRequestHandler<RefreshTokenCommand,LoginResponse>
    {
        private readonly IJWTService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;

        public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = request.JwtToken;
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            var principal = _jwtService.GetTokenPrincipal(token);
            var response = new LoginResponse();

            if (principal?.Identity?.Name is null)
            {
                throw new BadRequestException("Invalid token");
            }

            var identityUser = await _unitOfWork.UserRepository.GetTokenModel(principal.Identity.Name);

            if (identityUser is null || string.IsNullOrEmpty(identityUser.refreshToken) || identityUser.refreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid token");
            }

            var user = await _unitOfWork.CacheRepository.GetCacheData<User>($"user-{identityUser.id}");

            if (user is null)
            {
                var tempUser = _mapper.Map<UserDTO>(await _unitOfWork.UserRepository.GetUserById(identityUser.id));
                
                if(tempUser is null)
                {
                    throw new NotFoundException("User not found");
                }

                response.User = tempUser;
                

                await _unitOfWork.CacheRepository.SetCatcheData($"user-{identityUser.id}", response.User, new TimeSpan(24, 0, 0));
                
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                response.User = _mapper.Map<UserDTO>(user);
            }

            response.IsLoggedIn = true;
            response.JwtToken = _jwtService.GenerateJwtToken(identityUser.email);
            refreshToken = _jwtService.GenerateRefreshToken();

            var identityUserTokenModel = await _unitOfWork.UserRepository.GetTokenModel(identityUser.email);

            if (identityUserTokenModel is null)
            {
                await _unitOfWork.UserRepository.AddRefreshTokenField(new RefreshTokenModel
                {
                    email = identityUser.email,
                    refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12),
                    userId = identityUser.id,
                    user = null
                });
                
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                identityUserTokenModel.refreshToken = refreshToken;
                identityUserTokenModel.refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);
            }

            response.RefreshToken = refreshToken;

            await _unitOfWork.UserRepository.UpdateRefreshTokenAsync(identityUserTokenModel);

            await _unitOfWork.CompleteAsync();

            return response;
        }
    }
}
