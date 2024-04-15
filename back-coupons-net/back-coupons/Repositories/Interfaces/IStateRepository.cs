using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IStateRepository
    {
        Task<ActionResponse<IEnumerable<State>>> GetAsync();
        Task<ActionResponse<State>> GetAsync(int id);
    }
}
