﻿using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;
using Microsoft.AspNetCore.Identity;

namespace back_coupons.UnitsOfWork.Interfaces
{
    public interface IUserUnitOfWork
    {
        Task<User> GetUserAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<SignInResult> LoginAsync(LoginDTO model);
        Task LogoutAsync();
        Task<User> GetUserAsync(Guid userId);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
        Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination);
    }
}
