using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/coupons")]
    //[Route("api/[controller]")]
    public class CouponController : GenericController<Coupon>
    {
        private readonly ICouponUnitOfWork _couponUnitOfWork;
        public CouponController(IGenericUnitOfWork<Coupon> unit, ICouponUnitOfWork couponUnitOfWork) : base(unit)
        {
            _couponUnitOfWork = couponUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsyncFull()
        {
            var response = await _couponUnitOfWork.GetAsyncFull();
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _couponUnitOfWork.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }


        [HttpPost("redeem")]
        public async Task<IActionResult> redeem([FromQuery] string code)
        {
            var response = await _couponUnitOfWork.RedeemCouponAsync(code);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound(new { message = response.Message });
        }
    }
}
