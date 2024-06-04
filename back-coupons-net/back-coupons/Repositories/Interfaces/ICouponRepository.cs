using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface ICouponRepository
    {
        Task<ActionResponse<IEnumerable<Coupon>>> GetAsyncFull();
        Task<ActionResponse<IEnumerable<Coupon>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<Coupon>> RedeemCouponAsync(string codeCoupon);
    }
}
