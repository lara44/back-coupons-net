using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces

{
    public interface IProductUnitOfWork
    {
        Task<ActionResponse<IEnumerable<Product>>> GetAllAsync(int CompanyId);
        Task<ActionResponse<IEnumerable<Product>>> GetAllPaginationAsync(int CompanyId, PaginationDTO pagination);
        Task<ActionResponse<Product>> GetByIdAsync(int id);
    }
}
