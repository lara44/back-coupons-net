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
        public async Task<ActionResponse<IEnumerable<ClaimedCouponDto>>> GetRedeemedCouponsByClient(DateTime startDate, DateTime endDate, int companyId, RedeemState? state = null) 
            => await _reportCouponRepository.GetRedeemedCouponsByClient(startDate, endDate, companyId, state);

        public async Task<ActionResponse<IEnumerable<ClientRedemptionCountDto>>> GetCountRedeemedCouponsByClient(DateTime startDate, DateTime endDate, int companyId, RedeemState? state = null) 
            => await _reportCouponRepository.GetCountRedeemedCouponsByClient(startDate, endDate, companyId, state);

        public async  Task<ActionResponse<IEnumerable<CompanyRedemptionCountDto>>> GetCountRedeemedCouponsByCompany(DateTime startDate, DateTime endDate, int companyId, RedeemState? state)
            => await _reportCouponRepository.GetCountRedeemedCouponsByCompany(startDate, endDate, companyId, state);
    }
}
