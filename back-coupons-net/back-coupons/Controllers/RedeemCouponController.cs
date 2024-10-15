
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/redeem")]
    public class RedeemCouponController : Controller
    {
        private readonly IRedeemCouponUnitOfWork _redeemCouponUnitOfWork;
        public RedeemCouponController(IGenericUnitOfWork<RedeemCoupon> unit, IRedeemCouponUnitOfWork redeemCouponUnitOfWork)
        {
            _redeemCouponUnitOfWork = redeemCouponUnitOfWork;
        }
 
        [HttpPost("claimCustomerCoupon")]
        public async Task<IActionResult> redeem([FromQuery] string code, int clientId)
        {
            var response = await _redeemCouponUnitOfWork.RedeemCouponAsync(code, clientId);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound(new { message = response.Message });
        }
    }
}
