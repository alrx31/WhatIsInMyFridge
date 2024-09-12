using Application.Exceptions;
using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.Handlers.Queries
{
    public class GetUserByIdQueryHandler(
        IUnitOfWork unitOfWork
        ): IRequestHandler<GetUserQueryByIdQuery,User>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<User> Handle(GetUserQueryByIdQuery request, CancellationToken cancellationToken)
        {
            var user1 = await _unitOfWork.CacheRepository.GetCacheData<User>($"user-{request.Id}");
            if (user1 != null)
            {
                return user1;
            }

            var user = await _unitOfWork.UserRepository.GetUserById(request.Id);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            return user;
        }
    }
}
