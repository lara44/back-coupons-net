using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces
{
    public interface ICountryUnitOfWork
    {
        Task<ActionResponse<IEnumerable<Country>>> GetAsync();
        Task<ActionResponse<Country>> GetAsync(int id);
    }
}
