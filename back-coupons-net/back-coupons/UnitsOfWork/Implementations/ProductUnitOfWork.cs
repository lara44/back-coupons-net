using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
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

        public async Task<ICollection<Product>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Product> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
    }
}
