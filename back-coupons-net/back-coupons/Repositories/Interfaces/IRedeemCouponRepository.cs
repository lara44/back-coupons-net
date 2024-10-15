using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IRedeemCouponRepository
    {
        // Task<ActionResponse<IEnumerable<RedeemCoupon>>> GetAsyncFull();
        // Task<ActionResponse<IEnumerable<RedeemCoupon>>> GetAsync(PaginationDTO pagination);
        // Task<ActionResponse<RedeemCoupon>> GetAsync(int id);
        Task<ActionResponse<RedeemCoupon>> RedeemCouponAsync(string codeCoupon, int clientId);
    }
}
