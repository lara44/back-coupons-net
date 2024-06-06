using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
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
                .Include(dc => dc.DetailCoupons!)
                .ThenInclude(p => p.Product)
                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<IEnumerable<Entities.Coupon>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Coupons
                .Include(dc => dc.DetailCoupons!)
                .ThenInclude(p => p.Product)
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

        public override async Task<ActionResponse<Entities.Coupon>> GetAsync(int id)
        {
            var row = await _dbContext.Coupons
                   .Include(dc => dc.DetailCoupons!)
                   .ThenInclude(p => p.Product)
                   .OrderBy(s => s.Name)
                   .FirstOrDefaultAsync(c => c.Id == id);

            if (row != null)
            {
                return new ActionResponse<Entities.Coupon>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<Entities.Coupon>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public async Task<ActionResponse<Entities.Coupon>> RedeemCouponAsync(string code)
        {
            var coupon = await _dbContext.Coupons
                   .Include(dc => dc.DetailCoupons!)
                   .ThenInclude(p => p.Product)
                   .Where(c => c.CouponCode == code)
                   .OrderBy(s => s.Name)
                   .FirstOrDefaultAsync();

            if (coupon != null)
            {
                if (coupon.QuantityActual > 0)
                {
                    coupon.QuantityActual--;
                }
                else
                {
                    return new ActionResponse<Coupon>
                    {
                        Successfully = false,
                        Message = "No more coupons available."
                    };
                }

                await _dbContext.SaveChangesAsync();
                return new ActionResponse<Entities.Coupon>
                {
                    Successfully = true,
                    Result = coupon
                };
            }
            return new ActionResponse<Entities.Coupon>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }
    }
}