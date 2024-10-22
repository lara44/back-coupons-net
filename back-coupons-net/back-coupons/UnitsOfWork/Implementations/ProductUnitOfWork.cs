using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class ProductUnitOfWork : IProductUnitOfWork
    {
        private readonly IProductRepository _repository;

        public ProductUnitOfWork(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResponse<IEnumerable<Product>>> GetAllAsync(int CompanyId) => await _repository.GetAllAsync(CompanyId);
        public async Task<ActionResponse<IEnumerable<Product>>> GetAllPaginationAsync(int CompanyId, PaginationDTO pagination) => await _repository.GetAllPaginationAsync(CompanyId, pagination);
        public async Task<ActionResponse<Product>> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
    }
}
