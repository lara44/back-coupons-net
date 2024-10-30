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

        public async Task<ActionResponse<IEnumerable<ClaimedCouponDto>>> GetRedeemedCouponsByClient(
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

        public async Task<ActionResponse<IEnumerable<ClientRedemptionCountDto>>> GetCountRedeemedCouponsByClient(
            DateTime? startDate,
            DateTime? endDate,
            int? companyId = 0,
            RedeemState? state = null
        )
        {
            startDate = (startDate == DateTime.MinValue) ? null : startDate;
            endDate = (endDate == DateTime.MinValue) ? null : endDate;

            var claimedCoupons = await _dbContext.RedeemCoupons
                .Include(rc => rc.Client)
                .Include(rc => rc.Coupon)
                .Where(rc =>
                    (!startDate.HasValue || rc.DateRedeem.Date >= startDate.Value.Date) &&
                    (!endDate.HasValue || rc.DateRedeem.Date <= endDate.Value.Date) &&
                    (companyId == 0 || rc.Coupon!.CompanyId == companyId) &&
                    (state == null || rc.State == state))
                .GroupBy(rc => new 
                {
                    rc.Client!.Id,
                    rc.Client.FirstName,
                    rc.Client.LastName,
                    rc.Client.Email,
                    rc.Client.Phone
                })
                .Select(group => new ClientRedemptionCountDto
                {
                    ClientId = group.Key.Id,
                    FirstName = group.Key.FirstName,
                    LastName = group.Key.LastName,
                    Email = group.Key.Email,
                    Phone = group.Key.Phone,
                    RedemptionCount = group.Count() // Cuenta las redenciones por cliente
                })
                .ToListAsync();

            return new ActionResponse<IEnumerable<ClientRedemptionCountDto>>
            {
                Successfully = true,
                Result = claimedCoupons
            };
        }

        public async Task<ActionResponse<IEnumerable<CompanyRedemptionCountDto>>> GetCountRedeemedCouponsByCompany (
            DateTime? startDate,
            DateTime? endDate,
            int? companyId = 0,
            RedeemState? state = null
        )
        {
            startDate = (startDate == DateTime.MinValue) ? null : startDate;
            endDate = (endDate == DateTime.MinValue) ? null : endDate;

            var claimedCoupons = await _dbContext.RedeemCoupons
                .Include(rc => rc.Client)
                .Include(rc => rc.Coupon)
                .Where(rc =>
                    (!startDate.HasValue || rc.DateRedeem.Date >= startDate.Value.Date) &&
                    (!endDate.HasValue || rc.DateRedeem.Date <= endDate.Value.Date) &&
                    (companyId == 0 || rc.Coupon!.CompanyId == companyId) &&
                    (state == null || rc.State == state))
                .GroupBy(rc => new 
                {
                    rc.Coupon!.CompanyId,
                    rc.Coupon.Company!.Name,
                    rc.Coupon.Company.Nit
                })
                .Select(group => new CompanyRedemptionCountDto
                {
                    CompanyId = group.Key.CompanyId,
                    Name = group.Key.Name,
                    Nit = group.Key.Nit,
                    RedemptionCount = group.Count()
                })
                .ToListAsync();

            return new ActionResponse<IEnumerable<CompanyRedemptionCountDto>>
            {
                Successfully = true,
                Result = claimedCoupons
            };
        }
    }
}