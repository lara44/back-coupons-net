using back_coupons.DTOs;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces
{
    public interface IGenericUnitOfWork<T> where T : class
    {
        Task<ActionResponse<IEnumerable<T>>> GetAsyncFull();
        Task<ActionResponse<T>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
        Task<ActionResponse<T>> AddAsync(T model);
        Task<ActionResponse<T>> UpdateAsync(int id, T updatedModel);
        Task<ActionResponse<T>> DeleteAsync(int id);
        
    }
}
