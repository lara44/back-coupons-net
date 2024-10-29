using System.Data;
using back_coupons.Data;
using back_coupons.DTOs.Response;
using back_coupons.Enums;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class ReportCouponRepository : IReportCouponRepository
    {
        private readonly DataContext _dbContext;
        public ReportCouponRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActionResponse<IEnumerable<ClaimedCouponDto>>> GetClaimedCouponsByDateAndCompany(
            DateTime? startDate,
            DateTime? endDate,
            int? companyId = 0,
            RedeemState? state = null
        )
        {
            startDate = (startDate == DateTime.MinValue) ? null : startDate;
            endDate = (endDate == DateTime.MinValue) ? null : endDate;

            var claimedCoupons = await _dbContext.RedeemCoupons
                .Include(rc => rc.Coupon)
                .Include(rc => rc.Client)
                .Where(rc =>
                    (!startDate.HasValue || rc.DateRedeem.Date >= startDate.Value.Date) &&
                    (!endDate.HasValue || rc.DateRedeem.Date <= endDate.Value.Date) &&
                    (rc.Coupon!.CompanyId == companyId || rc.Coupon!.CompanyId > 0 && companyId == 0) &&
                    (rc.State == state || state == null))
                .OrderBy(rc => rc.DateRedeem)
                .Select(rc => new ClaimedCouponDto
                {
                    Id = rc.Id,
                    DateRedeem = rc.DateRedeem,
                    Coupon = new CouponBasicDto
                    {
                        Id = rc.Coupon!.Id,
                        Name = rc.Coupon.Name,
                        CouponCode = rc.Coupon.CouponCode
                    },
                    Client = new ClientBasicDto
                    {
                        Id = rc.Client!.Id,
                        FirstName = rc.Client.FirstName,
                        LastName = rc.Client.LastName,
                        Email = rc.Client.Email,
                        Phone = rc.Client.Phone
                    }
                })
                .ToListAsync();

            return new ActionResponse<IEnumerable<ClaimedCouponDto>>
            {
                Successfully = true,
                Result = claimedCoupons
            };
        }
    }
}