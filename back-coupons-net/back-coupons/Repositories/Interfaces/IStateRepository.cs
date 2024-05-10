using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IStateRepository
    {
        Task<ActionResponse<IEnumerable<State>>> GetAsyncFull();
        Task<ActionResponse<State>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<IEnumerable<State>>> GetStatesByCountryListAsync(int country);
    }
}
