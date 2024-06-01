using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Helpers;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class CouponRepository : GenericRepository<Entities.Coupon>, ICouponRepository
    {
        private readonly DataContext _dbContext;
        public CouponRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<ActionResponse<IEnumerable<Entities.Coupon>>> GetAsyncFull()
        {
            return new ActionResponse<IEnumerable<Entities.Coupon>>
            {
                Successfully = true,
                Result = await _dbContext.Coupons
                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<IEnumerable<Entities.Coupon>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Coupons
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Entities.Coupon>>
            {
                Successfully = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<IEnumerable<Entities.Coupon>>> GetCouponByCodeAsync(string code)
        {
            var result = await _dbContext.Coupons
                   .Where(c => c.CouponCode == code)
                   .OrderBy(s => s.Name)
                   .ToListAsync();

            return new ActionResponse<IEnumerable<Entities.Coupon>>
            {
                Successfully = true,
                Result = result
            };
        }
    }
}