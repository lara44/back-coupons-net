
using back_coupons.DTOs.Response;
using back_coupons.Entities;
using back_coupons.Enums;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IRedeemCouponUnitOfWork
    {
        Task<ActionResponse<RedeemCoupon>> ClaimCustomerCouponAsync(string codeCoupon, int clientId);
        Task<ActionResponse<RedeemCoupon>> RedeemCouponAsync(int couponId, int clientId, string signature);
        Task<ActionResponse<IEnumerable<ClaimedCouponClientDto>>> GetCouponsByClientAsync(string identification);
          Task<ActionResponse<IEnumerable<ClaimedCouponDto>>> GetClaimedCouponsByDateAndCompany(DateTime startDate, DateTime endDate, int companyId, RedeemState? state = null);
    }
}
