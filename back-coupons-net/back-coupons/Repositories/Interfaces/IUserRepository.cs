using back_coupons.DTOs;
using back_coupons.Entities;
using Microsoft.AspNetCore.Identity;

namespace back_coupons.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<SignInResult> LoginAsync(LoginDTO model);
        Task LogoutAsync();
    }
}
