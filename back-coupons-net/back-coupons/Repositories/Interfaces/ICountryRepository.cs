using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<ActionResponse<IEnumerable<Country>>> GetAsyncFull();
        Task<ActionResponse<Country>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<IEnumerable<Country>>> GetCountryListAsync();
    }
}
