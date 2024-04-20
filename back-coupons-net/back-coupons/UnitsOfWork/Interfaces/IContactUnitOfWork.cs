using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces
{
    public interface IContactUnitOfWork
    {
        Task<ActionResponse<IEnumerable<Contact>>> GetAsyncFull();
        Task<ActionResponse<Contact>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<Contact>>> GetAsync(PaginationDTO pagination);
    }
}
