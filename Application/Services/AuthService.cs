
using Application.DTO;
using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAuthService
    {

        Task<LoginResponse> LoginUser(LoginDTO model);
        Task Logout(LogoutDTO model);
        Task<LoginResponse> RefreshToken(RefreshTokenDTO model);
        Task RegisterUser(RegisterDTO model);
    
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IJWTService _jwtService;
        private readonly IUnitOfWork _unitOfWork;


        public AuthService(IUserRepository repository,IJWTService jwtService,IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _repository = repository;
            _unitOfWork = unitOfWork;

        }

        public async Task<LoginResponse> LoginUser(LoginDTO user)
        {

            var response = new LoginResponse();

            var identifyUser = await _repository.GetUserByLogin(user.login);
            
            if (identifyUser is null ||
                (identifyUser.password == getHash(user.password)) == false)
            {
                return response;
            };
            
            response.IsLoggedIn = true;
            response.UserId = identifyUser.id;
            response.JwtToken = _jwtService.GenerateJwtToken(identifyUser.email);
            response.RefreshToken = _jwtService.GenerateRefreshToken();
            
            var identityUserTokenModel = await _repository.getTokenModel(identifyUser.email);
            
            if(identityUserTokenModel is null)
            {
                await _repository.AddRefreshTokenField(new RefreshTokenModel
                {
                    email = identifyUser.email,
                    refreshToken = response.RefreshToken,
                    refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12),
                    userId = identifyUser.id,
                    user = null
                });
            }
            else
            {
                identityUserTokenModel.refreshToken = response.RefreshToken;
                identityUserTokenModel.refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);
            }

            await _repository.UpdateRefreshTokenAsync(identityUserTokenModel);
            
            await _unitOfWork.CompleteAsync();
            
            return response;
            
        }

        public async Task Logout(LogoutDTO model)
        {

            if (model.Token == null || model.UserId <= 0)
            {
                throw new ValidationException("Invalid token or user id");
            }
            
            await _repository.CanselRefreshToken(model.UserId);
            
            await _unitOfWork.CompleteAsync();
        }

        public async Task<LoginResponse> RefreshToken(RefreshTokenDTO model)
        {
            var principal = _jwtService.GetTokenPrincipal(model.JwtToken);
            var response = new LoginResponse();
            
            if (principal?.Identity?.Name is null)
            {
                return response;
            }

            var identityUser = await _repository.getTokenModel(principal.Identity.Name);
            
            if (identityUser is null || string.IsNullOrEmpty(identityUser.refreshToken) || identityUser.refreshTokenExpiryTime < DateTime.UtcNow)
            {
                return response;
            }

            response.UserId = identityUser.id;
            response.IsLoggedIn = true;
            response.JwtToken = _jwtService.GenerateJwtToken(identityUser.email);
            response.RefreshToken = _jwtService.GenerateRefreshToken();
            var identityUserTokenModel =
                await _repository.getTokenModel(identityUser.email);
            
            if (identityUserTokenModel is null)
            {
                await _repository.AddRefreshTokenField(new RefreshTokenModel
                {
                    email = identityUser.email,
                    refreshToken = response.RefreshToken,
                    refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12),
                    userId = identityUser.id,
                    user = null
                });
            }
            else
            {
                identityUserTokenModel.refreshToken = response.RefreshToken;
                identityUserTokenModel.refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);
            }

            await _repository.UpdateRefreshTokenAsync(identityUserTokenModel);
            
            await _unitOfWork.CompleteAsync();
            
            return response;
        }

        public async Task RegisterUser(RegisterDTO model)
        {

            await _repository.RegisterUser(new User
            {
                name = model.name,
                login = model.login,
                email = model.email,
                password = getHash(model.password),
                isAdmin = model.name == "admin"
            });
            
            await _unitOfWork.CompleteAsync();

            var user = await _repository.GetUserByLogin(model.login);

            if (user == null) { 
                throw new ValidationException("User not found");
            }

            var newToken = new RefreshTokenModel
            {
                email = model.email,
                refreshToken = "",
                refreshTokenExpiryTime = DateTime.UtcNow,
                userId = user.id,
                user = null
            };

            await _repository.AddRefreshTokenField(newToken);
            
            await _unitOfWork.CompleteAsync();
        }


        private string getHash(string pass)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(pass);
            data = System.Security.Cryptography.SHA256.HashData(data);
            
            return Encoding.ASCII.GetString(data);
        }
    }
}
