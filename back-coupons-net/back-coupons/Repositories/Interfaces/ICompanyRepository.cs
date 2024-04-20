using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<ActionResponse<IEnumerable<Company>>> GetAsyncFull();
        Task<ActionResponse<Company>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<Company>>> GetAsync(PaginationDTO pagination);
    }
}
