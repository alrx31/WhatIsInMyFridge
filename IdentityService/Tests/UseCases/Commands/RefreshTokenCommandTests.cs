using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.Handlers.Comands;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Infastructure.Persistanse;
using Infastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Tests.UseCases.Commands
{
    public class RefreshTokenCommandTests
    {
        private readonly Mock<IJWTService> _jwtService;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICacheRepository> _cacheRepository;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IMapper> _mapper;

        private readonly RefreshTokenCommandHandler _handler;
    
        public RefreshTokenCommandTests()
        {
            _jwtService = new Mock<IJWTService>();
            _userRepository = new Mock<IUserRepository>();
            _cacheRepository = new Mock<ICacheRepository>();
            _mapper = new Mock<IMapper>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.UserRepository).Returns(_userRepository.Object);
            _unitOfWork.SetupGet(x => x.CacheRepository).Returns(_cacheRepository.Object);

            var context = new DefaultHttpContext();
           
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            _handler = new RefreshTokenCommandHandler(
                _jwtService.Object,
                _unitOfWork.Object,
                _httpContextAccessor.Object,
                _mapper.Object
            );
        }

        
    }
}
