using back_coupons.Entities;

namespace back_coupons.UnitsOfWork.Interfaces

{
    public interface IUserUnitOfWork
    {
        Task<ICollection<User>> GetAllAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
    }
}
