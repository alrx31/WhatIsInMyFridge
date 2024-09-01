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
    public class UpdateUserCommandHandler(

        IUserRepository userRepository,
        IJWTService jwtService,
        ICacheRepository cacheRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper

        ) : IRequestHandler<UpdateUserCommand,User>
    {

        private readonly IUserRepository _repository = userRepository;
        private readonly IJWTService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheRepository _cacheRepository = cacheRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var model = request.user;
            var user = await _cacheRepository.GetCacheData<User>($"user-{request.id}");

            if (user is null)
            {
                user = await _repository.getUserById(request.id);
            }

            if (user is null)
            {
                throw new NotFoundException("User not Found");
            }

            user.login = model.Login;
            user.password = Scripts.GetHash(model.Password);
            user.email = model.Email;
            user.name = model.Name;
            user.id = request.id;
            User user1 = await _repository.UpdateUser(user);

            if (user1 is null)
            {
                throw new NotFoundException("Invalid User");
            }

            await _unitOfWork.CompleteAsync();

            await _cacheRepository.SetCatcheData($"user-{request.id}", user1);

            return user1;
        }

    }
}
