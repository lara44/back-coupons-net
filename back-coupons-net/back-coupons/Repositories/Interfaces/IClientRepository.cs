using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<ActionResponse<IEnumerable<Client>>> GetAsyncFull();
        Task<ActionResponse<Client>> GetAsync(int id);
        Task<ActionResponse<Client>> GetClientByIdentificationAsync(string identification);
        Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination);
    }
}
