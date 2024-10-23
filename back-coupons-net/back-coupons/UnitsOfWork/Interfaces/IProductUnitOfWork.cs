using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces

{
    public interface IProductUnitOfWork
    {
        Task<ActionResponse<IEnumerable<Product>>> GetAllFullAsync(int CompanyId);
        Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<Product>> GetAsync(int id);
    }
}
