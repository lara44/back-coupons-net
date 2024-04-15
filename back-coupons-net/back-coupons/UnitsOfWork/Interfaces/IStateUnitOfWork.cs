using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces
{
    public interface IStateUnitOfWork
    {
        Task<ActionResponse<IEnumerable<State>>> GetAsync();
        Task<ActionResponse<State>> GetAsync(int id);
    }
}
