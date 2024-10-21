using back_coupons.Data;
using back_coupons.Entities;
using back_coupons.Exceptions;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dbContext;

        public ProductRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task<ActionResponse<IEnumerable<Product>>> GetAllAsync(int CompanyId)
        {
            var response = await _dbContext.Products
                .Where(p => p.CompanyId == CompanyId)
                .ToListAsync();

            return new ActionResponse<IEnumerable<Product>>
            {
                Successfully = true,
                Result = response
            };
        }

        public async Task<ActionResponse<Product>> GetByIdAsync(int id)
        {
            var response = await _dbContext.Products.FindAsync(id);
            if (response == null)
            {
                return new ActionResponse<Product>
                {
                    Successfully = false,
                    Message = $"Product con ID {id} no encontrado"
                };

            }

            return new ActionResponse<Product>
            {
                Successfully = true,
                Result = response
            };
        }
    }
}
