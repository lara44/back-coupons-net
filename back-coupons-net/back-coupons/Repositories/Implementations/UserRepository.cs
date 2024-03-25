using back_coupons.Data;
using back_coupons.Entities;
using back_coupons.Exceptions;
using back_coupons.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                var existingUser = await _dbContext.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                // Actualiza las propiedades del usuario existente con las del usuario actualizado.
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                // Guarda los cambios asincrónicamente en la base de datos.
                await _dbContext.SaveChangesAsync();
                return existingUser;
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción que pueda ocurrir durante la actualización del usuario
                // y la relanza envolviéndola en una nueva excepción con el mismo mensaje.
                throw new Exception("Error al actualizar usuario", ex);
            }
        }
    }
}
