﻿using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Implementations;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
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
        public async Task<User> GetUserAsync(Guid userId) => await _userRepository.GetUserAsync(userId);
        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword) => await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);
        public async Task<IdentityResult> UpdateUserAsync(User user) => await _userRepository.UpdateUserAsync(user);
        public async Task<string> GenerateEmailConfirmationTokenAsync(User user) => await _userRepository.GenerateEmailConfirmationTokenAsync(user);
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token) => await _userRepository.ConfirmEmailAsync(user, token);
        public async Task<string> GeneratePasswordResetTokenAsync(User user) => await _userRepository.GeneratePasswordResetTokenAsync(user);
        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password) => await _userRepository.ResetPasswordAsync(user, token, password);
        public async Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination) => await _userRepository.GetAsync(pagination);
    }
}
