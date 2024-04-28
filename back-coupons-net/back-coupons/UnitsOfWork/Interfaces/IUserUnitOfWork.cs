﻿using back_coupons.Entities;
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
    }
}