﻿
using Domain.Entities;
using Domain.Repository;
using Infastructure.Middlewares.Exceptions;
using Infastructure.Persistanse;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infastructure.Repository
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
            
            user!.refreshTokenExpiryTime = DateTime.UtcNow;
        }


        public async Task DeleteUser(int id)
        {
            // remove user with id equal id
            var user = await _context.users.FirstOrDefaultAsync(u => u.id == id);
            if(user!= null)
            {
                _context.users.Remove(user);

            }
            else
            {
                throw new NotFoundException("user not found");
            }

        }

        public async Task<RefreshTokenModel?> getTokenModel(string email)
        {

            return await _context.refreshTokens.FirstOrDefaultAsync(x => x.email == email);    
        
        }

        public Task<User?> getUserById(int id)
        {
            
            return _context.users.FirstOrDefaultAsync(x => x.id == id);
        
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
            
            if(user != null)
            {
                _context.refreshTokens.Update(user);
            }
        }

        public async Task<User> UpdateUser(User model, int id)
        {

            var user = await _context.users.FirstOrDefaultAsync(x=>x.id == id);
            if(user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.email = model.email;
            user.login = model.login;
            user.password = model.password;
            user.name = model.name;
            
            return user;
            
        }
    }
}
