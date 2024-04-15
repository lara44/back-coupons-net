using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<ActionResponse<IEnumerable<Country>>> GetAsync();
        Task<ActionResponse<Country>> GetAsync(int id);
    }
}
