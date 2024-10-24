using back_coupons.DTOs;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericUnitOfWork(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsyncFull() => await _repository.GetAsyncFull();
        public virtual async Task<ActionResponse<T>> GetAsync(int id) => await _repository.GetAsync(id);
        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination) => await _repository.GetAsync(pagination);
        public virtual async Task<ActionResponse<T>> AddAsync(T model) => await _repository.AddAsync(model);
        public virtual async Task<ActionResponse<T>> UpdateAsync(int id, T updatedModel) => await _repository.UpdateAsync(id, updatedModel);
        public virtual async Task<ActionResponse<T>> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
