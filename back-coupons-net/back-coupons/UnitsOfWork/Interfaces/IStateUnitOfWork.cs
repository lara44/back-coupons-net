using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces
{
    public interface IStateUnitOfWork
    {
        Task<ActionResponse<IEnumerable<State>>> GetAsyncFull();
        Task<ActionResponse<State>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<IEnumerable<State>>> GetStatesByCountryListAsync(int country);
    }
}
