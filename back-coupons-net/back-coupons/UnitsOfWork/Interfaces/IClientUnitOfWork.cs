using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces
{
    public interface IClientUnitOfWork
    {
        Task<ActionResponse<IEnumerable<Client>>> GetAsyncFull();
        Task<ActionResponse<Client>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination);
    }
}
