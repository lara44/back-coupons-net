
using back_coupons.Enums;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportCouponCouponController : Controller
    {
        private readonly IReportCouponUnitOfWork _reportCouponUnitOfWork;
        public ReportCouponCouponController(IReportCouponUnitOfWork reportCouponUnitOfWork)
        {
            _reportCouponUnitOfWork = reportCouponUnitOfWork;
        }

        [HttpGet("GetRedeemedCouponsByClient")]
        public async Task<IActionResult> GetRedeemedCouponsByClient(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int companyId,
            [FromQuery] RedeemState? state = null
        )
        {
            var response = await _reportCouponUnitOfWork.GetRedeemedCouponsByClient(startDate, endDate, companyId, state);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound(new { message = response.Message });
        }

        [HttpGet("GetCountRedeemedCouponsByClient")]
        public async Task<IActionResult> GetCountRedeemedCouponsByClient(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int companyId,
            [FromQuery] RedeemState? state = null
        )
        {
            var response = await _reportCouponUnitOfWork.GetCountRedeemedCouponsByClient(startDate, endDate, companyId, state);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound(new { message = response.Message });
        }

         [HttpGet("GetCountRedeemedCouponsByCompany")]
        public async Task<IActionResult> GetCountRedeemedCouponsByCompany(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int companyId,
            [FromQuery] RedeemState? state = null
        )
        {
            var response = await _reportCouponUnitOfWork.GetCountRedeemedCouponsByCompany(startDate, endDate, companyId, state);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound(new { message = response.Message });
        }
    }
}
