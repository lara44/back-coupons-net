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

        public async Task<ActionResponse<IEnumerable<Product>>> GetAllFullAsync(int CompanyId) => await _repository.GetAllFullAsync(CompanyId);
        public async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination) => await _repository.GetAsync(pagination);
        public async Task<ActionResponse<Product>> GetAsync(int id) => await _repository.GetAsync(id);
    }
}
