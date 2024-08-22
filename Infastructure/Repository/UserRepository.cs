using DataAcess.DTO;
using DataAcess.Entities;
using Infastructure.Persistanse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repository
{
    public interface IUserRepository
    {
        Task RegisterUser(RegisterDTO model);
        Task<User> GetUserByLogin(string email);
        Task<bool> CheckPasswordAsync(User identifyUser, string password);
        Task<RefreshTokenModel> getTokenModel(string email);
        Task UpdateRefreshTokenAsync(RefreshTokenModel identityUserTokenModel);
        Task AddRefreshTokenField(RegisterDTO model);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRefreshTokenField(RegisterDTO model)
        {
            var newToken = new RefreshTokenModel
            {
                email = model.email,
                refreshToken = "",
                refreshTokenExpiryTime = DateTime.UtcNow,
                userId = _context.users
                    .Where(p => p.email == model.email)
                    .Select(p => p.id)
                    .FirstOrDefault(),
                user = null
            };
            
            await _context.refreshTokens.AddAsync(newToken);

        }

        public Task<bool> CheckPasswordAsync(User identifyUser, string password)
        {
            return Task.FromResult(identifyUser.password == getHash(password));
        }

        public async Task<RefreshTokenModel> getTokenModel(string email)
        {
            return await _context.refreshTokens.FirstOrDefaultAsync(x => x.email == email);    
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.login == login);
        }

        public async Task RegisterUser(RegisterDTO model)
        {
            await _context.AddAsync(new User
            {
                name = model.name,
                login = model.login,
                email = model.email,
                password = getHash(model.password),
                isAdmin = model.name == "admin"
            });
        }

        public async Task UpdateRefreshTokenAsync(RefreshTokenModel identityUserTokenModel)
        {
            var user = await _context.refreshTokens.FirstOrDefaultAsync(u => u.email == identityUserTokenModel.email);
            _context.refreshTokens.Update(user);
        }

        private string getHash(string pass)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(pass);
            data = System.Security.Cryptography.SHA256.HashData(data);
            return Encoding.ASCII.GetString(data);
        }

    }
}
