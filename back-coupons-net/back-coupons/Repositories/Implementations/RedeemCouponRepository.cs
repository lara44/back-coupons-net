using back_coupons.Data;
using back_coupons.Entities;
using back_coupons.Enums;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class RedeemCouponRepository :  IRedeemCouponRepository
    {
        private readonly DataContext _dbContext;
        public RedeemCouponRepository(DataContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<ActionResponse<RedeemCoupon>> RedeemCouponAsync(string code, int clientId)
        {
            var client = await _dbContext.Clients
                .Where(c => c.Id == clientId)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "Client not found."
                };
            }

            var coupon = await _dbContext.Coupons
                .Include(dc => dc.DetailCoupons!)
                .ThenInclude(p => p.Product)
                .Where(c => c.CouponCode == code)
                .OrderBy(s => s.Name)
                .FirstOrDefaultAsync();

            // Si se encuentra el cupón
            if (coupon != null)
            {
                // Verificar si hay cantidad disponible
                if (coupon.QuantityActual > 0)
                {
                    coupon.QuantityActual--;

                    // Crear el registro de redención del cupón
                    var redeemCoupon = new RedeemCoupon
                    {
                        CouponId = coupon.Id,
                        ClientId = clientId,
                        DateRedeem = DateTime.UtcNow,
                        State = RedeemState.Pending
                    };

                    _dbContext.RedeemCoupons.Add(redeemCoupon);
                    await _dbContext.SaveChangesAsync();

                    return new ActionResponse<RedeemCoupon>
                    {
                        Successfully = true,
                        Result = redeemCoupon
                    };
                }
                else
                {
                    // Si no hay más cupones disponibles
                    return new ActionResponse<RedeemCoupon>
                    {
                        Successfully = false,
                        Message = "No more coupons available."
                    };
                }
            }

            // Si el cupón no fue encontrado
            return new ActionResponse<RedeemCoupon>
            {
                Successfully = false,
                Message = "Coupon not found."
            };
        }
    }
}