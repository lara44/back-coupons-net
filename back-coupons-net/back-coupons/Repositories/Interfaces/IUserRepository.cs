using back_coupons.Entities;

namespace back_coupons.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllAsync();

        Task<User> GetUserByIdAsync(int userId);

        Task<User> CreateUserAsync(User User);
    }
}
