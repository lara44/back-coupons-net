using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Helpers;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _dbContext;

        public ProductRepository(DataContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<ActionResponse<IEnumerable<Product>>> GetAllFullAsync(int CompanyId)
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

        public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Products
                .Where(p => !pagination.Id.HasValue || p.CompanyId == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Product>>
            {
                Successfully = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public override async Task<ActionResponse<Product>> GetAsync(int id)
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
