using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface ICityRepository
    {
        Task<ActionResponse<IEnumerable<City>>> GetAsyncFull();
        Task<ActionResponse<City>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<IEnumerable<City>>> GetCitiesByStatesAsync(int state);
    }
}
