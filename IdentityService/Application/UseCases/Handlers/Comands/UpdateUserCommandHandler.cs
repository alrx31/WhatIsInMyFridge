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
    public class UpdateUserCommandHandler(

        IJWTService jwtService,
        IUnitOfWork unitOfWork,
        IMapper mapper

        ) : IRequestHandler<UpdateUserCommand,User> 
    {

        private readonly IJWTService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var model = request.user;
            var user = await _unitOfWork.CacheRepository.GetCacheData<User>($"user-{request.id}");

            if (user is null)
            {
                user = await _unitOfWork.UserRepository.GetUserById(request.id);
            }

            if (user is null)
            {
                throw new NotFoundException("User not Found");
            }

            user.email = model.Email;
            user.login = model.Login;
            user.name= model.Name;
            user.password = Scripts.GetHash(model.Password);

            User user1 = await _unitOfWork.UserRepository.UpdateUser(user);

            await _unitOfWork.CacheRepository.SetCatcheData($"user-{request.id}", user1);

            await _unitOfWork.CompleteAsync();

            return user1;
        }
    }
}
