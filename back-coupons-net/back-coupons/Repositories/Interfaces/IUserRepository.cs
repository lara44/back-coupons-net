﻿using back_coupons.Entities;
using Microsoft.AspNetCore.Identity;

namespace back_coupons.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
    }
}
