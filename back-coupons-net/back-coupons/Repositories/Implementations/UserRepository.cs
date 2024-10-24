using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Enums;
using back_coupons.Helpers;
using back_coupons.Migrations;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(
            DataContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager
        )
        {
            _dbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _dbContext.Users
                .Include(u => u.Company)
                .Include(u => u.City!)
                .ThenInclude(c => c.States!)
                .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(x => x.Email == email);
            return user!;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.City!)
                .ThenInclude(c => c.States!)
                .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(x => x.Id == userId.ToString());
            return user!;
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginDTO model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Users
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(u => u.Email.ToLower().Contains(pagination.Filter.ToLower()));
            }

            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);

            // Proyecta los resultados a una entidad UserDTO con los campos deseados
            var users = await queryable
                .Include(u => u.Company)
                .OrderBy(u => u.Email)
                .Paginate(pagination)
                .Select(u => new User
                {
                    Id = u.Id,
                    Document = u.Document,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Address = u.Address,
                    Photo = u.Photo,
                    Email = u.Email,
                    UserType = u.UserType,
                    CityId = u.CityId,
                    Company = u.Company,
                    CompanyId = u.CompanyId
                })
                .ToListAsync();

            return new ActionResponse<IEnumerable<User>>
            {
                Successfully = true,
                Result = users,
                TotalPage = totalPages
            };
        }
    }
}
