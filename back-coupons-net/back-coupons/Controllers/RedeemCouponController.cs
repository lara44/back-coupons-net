
using back_coupons.DTOs.Request;
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
 
        [HttpPost("ClaimCustomerCoupon")]
        public async Task<IActionResult> ClaimCustomerCoupon([FromBody] ClaimCouponRequest request)
        {
            var response = await _redeemCouponUnitOfWork.ClaimCustomerCouponAsync(request.Code, request.ClientId);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound(new { message = response.Message });
        }

        [HttpPost("RedeemCoupon")]
        public async Task<IActionResult> RedeemCoupon([FromBody] RedeemCouponRequest request)
        {
            var response = await _redeemCouponUnitOfWork.RedeemCouponAsync(request.CouponId, request.ClientId, request.Signature);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound(new { message = response.Message });
        }

        [HttpGet("GetCouponsByClientIdentification/{identification}")]
        public async Task<IActionResult> GetCouponsByClientIdentification(string identification)
        {
            var response = await _redeemCouponUnitOfWork.GetCouponsByClientAsync(identification);

            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound(new { message = response.Message });
        }
    }
}
