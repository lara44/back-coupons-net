using back_coupons.DTOs.Response;
using back_coupons.Enums;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IReportCouponRepository
    {
        Task<ActionResponse<IEnumerable<ClaimedCouponDto>>> GetClaimedCouponsByDateAndCompany(DateTime? startDate, DateTime? endDate, int? companyId = 0,  RedeemState? state = null);
    }
}
