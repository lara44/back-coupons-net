using back_coupons.Entities;
using back_coupons.Responses;
using Microsoft.AspNetCore.Identity;

namespace back_coupons.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<ActionResponse<IEnumerable<Product>>> GetAllAsync(int CompanyId);
        Task<ActionResponse<Product>> GetByIdAsync(int id);
    }
}
