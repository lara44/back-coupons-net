
using back_coupons.DTOs.Response;
using back_coupons.Enums;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Interfaces
{
    public interface IReportCouponUnitOfWork
    {
        Task<ActionResponse<IEnumerable<ClaimedCouponDto>>> GetClaimedCouponsByDateAndCompany(DateTime startDate, DateTime endDate, int companyId, RedeemState? state = null);
    }
}
