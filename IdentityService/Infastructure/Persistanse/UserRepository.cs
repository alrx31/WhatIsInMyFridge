
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Persistanse
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRefreshTokenField(RefreshTokenModel model)
        {
            await _context.refreshTokens.AddAsync(model);
        }

        public async Task CanselRefreshToken(int userId)
        {
            var user = await _context.refreshTokens
                .FirstOrDefaultAsync(u => u.userId == userId);

            user.refreshTokenExpiryTime = DateTime.UtcNow;
        }


        public async Task DeleteUser(int id)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.id == id);

            _context.users.Remove(user);
        }

        public async Task DeleteRefreshTokenByUserId(int userId)
        {
            var token = await _context.refreshTokens.FirstOrDefaultAsync(u => u.userId == userId);

            _context.refreshTokens.Remove(token);
        }


        public async Task<RefreshTokenModel?> GetTokenModel(string email)
        {
            return await _context.refreshTokens.FirstOrDefaultAsync(x => x.email == email);
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<User?> GetUserByLogin(string login)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.login == login);
        }

        public async Task RegisterUser(User model)
        {
            await _context.AddAsync(model);
        }

        public async Task UpdateRefreshTokenAsync(RefreshTokenModel identityUserTokenModel)
        {
            var user = await _context.refreshTokens.FirstOrDefaultAsync(u => u.email == identityUserTokenModel.email);

            _context.refreshTokens.Update(user);
        }

        public async Task<User> UpdateUser(User model)
        {
            _context.users.Update(model);
            return model;
        }

        public Task<List<User>> GetUsers(List<int> ids)
        {
            return Task.FromResult(_context.users.Where(u => ids.Contains(u.id)).ToList<User>());
        }
    }
}
