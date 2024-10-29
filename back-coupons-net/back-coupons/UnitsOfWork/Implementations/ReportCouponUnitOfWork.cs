using back_coupons.DTOs.Response;
using back_coupons.Enums;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class ReportCouponUnitOfWork : IReportCouponUnitOfWork
    {
        private readonly IReportCouponRepository _reportCouponRepository;
        public ReportCouponUnitOfWork(IReportCouponRepository reportCouponRepository)
        {
            _reportCouponRepository = reportCouponRepository;
        }
        public async Task<ActionResponse<IEnumerable<ClaimedCouponDto>>> GetClaimedCouponsByDateAndCompany(DateTime startDate, DateTime endDate, int companyId, RedeemState? state = null) 
            => await _reportCouponRepository.GetClaimedCouponsByDateAndCompany(startDate, endDate, companyId, state);
    }
}
