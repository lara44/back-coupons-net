
using back_coupons.Entities;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IRedeemCouponUnitOfWork
    {
        Task<ActionResponse<RedeemCoupon>> RedeemCouponAsync(string codeCoupon, int clientId);
    }
}
