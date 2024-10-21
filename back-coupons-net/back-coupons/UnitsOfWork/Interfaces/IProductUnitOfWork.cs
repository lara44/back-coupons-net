using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces

{
    public interface IProductUnitOfWork
    {
        Task<ActionResponse<IEnumerable<Product>>> GetAllAsync(int CompanyId);
        Task<ActionResponse<Product>> GetByIdAsync(int id);
    }
}
