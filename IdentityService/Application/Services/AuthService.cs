﻿
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Infastructure.Middlewares.Exceptions;
using Microsoft.AspNetCore.Http;
using Npgsql.Replication;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;


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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheRepository _cacheRepository;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository repository,
            IJWTService jwtService,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            ICacheRepository cacheRepository,
            IMapper mapper
            )
        {
            _jwtService = jwtService;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _cacheRepository = cacheRepository;
            _mapper = mapper;
        }

        public async Task<LoginResponse> LoginUser(LoginDTO user)
        {
            var response = new LoginResponse();

            var identifyUser = await _repository.GetUserByLogin(user.login);
            
            if (identifyUser is null ||
                (identifyUser.password == getHash(user.password)) == false)
            {

                //return response;
                throw new BadRequestException("Invalid login or password");
            };
            
            response.IsLoggedIn = true;
            response.User = _mapper.Map<UserDTO>(identifyUser);
            response.JwtToken = _jwtService.GenerateJwtToken(identifyUser.email);
            var RefreshToken = _jwtService.GenerateRefreshToken();
            
            var identityUserTokenModel = await _repository.getTokenModel(identifyUser.email);
            
            if(identityUserTokenModel is null)
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
            
            await _cacheRepository.SetCatcheData($"user-{response.User.id}", response.User,new TimeSpan(24,0,0));

            return response;
        }

        public async Task Logout(LogoutDTO model)
        {
            await _repository.CanselRefreshToken(model.UserId);
            
            await _unitOfWork.CompleteAsync();
        }

        public async Task<LoginResponse> RefreshToken(RefreshTokenDTO model)
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            

            var principal = _jwtService.GetTokenPrincipal(model.JwtToken);
            var response = new LoginResponse();
            
            if (principal?.Identity?.Name is null)
            {
                throw new BadRequestException("Invalid token");
                //return response;
            }

            var identityUser = await _repository.getTokenModel(principal.Identity.Name);
            
            if (identityUser is null || string.IsNullOrEmpty(identityUser.refreshToken) || identityUser.refreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid token");
                //return response;
            }

            var user = await _cacheRepository.GetCacheData<User>($"user-{identityUser.id}");

            if (user is null)
            {
                response.User = _mapper.Map<UserDTO>(await _repository.getUserById(identityUser.id) ?? throw new NotFoundException("User not found"));
                
                await _cacheRepository.SetCatcheData($"user-{identityUser.id}", response.User, new TimeSpan(24, 0, 0));   
            }
            else
            {
                response.User = _mapper.Map<UserDTO>(user);
            }

            response.IsLoggedIn = true;
            response.JwtToken = _jwtService.GenerateJwtToken(identityUser.email);
            refreshToken = _jwtService.GenerateRefreshToken();
            
            var identityUserTokenModel = await _repository.getTokenModel(identityUser.email);
            
            if (identityUserTokenModel is null)
            {

                await _repository.AddRefreshTokenField(new RefreshTokenModel
                {
                    email = identityUser.email,
                    refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12),
                    userId = identityUser.id,
                    user = null
                });

            }
            else
            {
                identityUserTokenModel.refreshToken = refreshToken;
                identityUserTokenModel.refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);
            }

            response.RefreshToken = refreshToken;

            await _repository.UpdateRefreshTokenAsync(identityUserTokenModel);
            
            await _unitOfWork.CompleteAsync();
            
            return response;
        }

        public async Task RegisterUser(RegisterDTO model)
        {
            var userCheck = await _repository.GetUserByLogin(model.login);

            if (userCheck!= null)
            {
                throw new AlreadyExistsException("This Login is not avaible");
            }

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

            if (user is null) { 
                throw new ValidationDataException("User not found");
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


        public string getHash(string pass)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(pass);
            data = System.Security.Cryptography.SHA256.HashData(data);
            
            return Encoding.ASCII.GetString(data);
        }
    }
}




/*
 Domain
Infastructure
Application
Presentation
 
 */