using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class UserUnitOfWork : IUserUnitOfWork
    {
        private readonly IUserRepository _userRepository;

        public UserUnitOfWork(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserAsync(string email) => await _userRepository.GetUserAsync(email);
        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _userRepository.AddUserAsync(user, password);
        public async Task CheckRoleAsync(string roleName) => await _userRepository.CheckRoleAsync(roleName);
        public async Task AddUserToRoleAsync(User user, string roleName) => await _userRepository.AddUserToRoleAsync(user, roleName);
        public async Task<bool> IsUserInRoleAsync(User user, string roleName) => await _userRepository.IsUserInRoleAsync(user, roleName);
        public async Task<SignInResult> LoginAsync(LoginDTO model) => await _userRepository.LoginAsync(model);
        public async Task LogoutAsync() => await _userRepository.LogoutAsync();
    }
}
