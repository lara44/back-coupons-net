
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IRedeemCouponUnitOfWork
    {
        Task<ActionResponse<RedeemCoupon>> ClaimCustomerCouponAsync(string codeCoupon, int clientId);
        Task<ActionResponse<RedeemCoupon>> RedeemCouponAsync(int couponId, int clientId, string signature);
        Task<ActionResponse<IEnumerable<RedeemCoupon>>> GetCouponsByClientAsync(string identification);
    }
}
