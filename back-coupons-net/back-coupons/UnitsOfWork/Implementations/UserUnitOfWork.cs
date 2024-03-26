using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class UserUnitOfWork : IUserUnitOfWork
    {
        private readonly IUserRepository _userRepository;

        public UserUnitOfWork(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ICollection<User>> GetAllAsync() => await _userRepository.GetAllAsync();
        public async Task<User> GetUserByIdAsync(int userId) => await _userRepository.GetUserByIdAsync(userId);
        public async Task<User> CreateUserAsync(User user) => await _userRepository.CreateUserAsync(user);
        public async Task<User> UpdateUserAsync(User user) => await _userRepository.UpdateUserAsync(user);
    }
}
