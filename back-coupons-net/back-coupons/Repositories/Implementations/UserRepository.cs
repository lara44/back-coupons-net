using back_coupons.Data;
using back_coupons.Entities;
using back_coupons.Exceptions;
using back_coupons.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace back_coupons.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbContext;

        public UserRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var response = await _dbContext.Users.FindAsync(userId);
            if (response == null)
            {
                throw new NotFoundException($"Usuario con ID {userId} no encontrado");
            }

            return response;
        }

        public async Task<User> CreateUserAsync(User User)
        {
            try
            {
                _dbContext.Users.Add(User);
                await _dbContext.SaveChangesAsync();
                return User;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
