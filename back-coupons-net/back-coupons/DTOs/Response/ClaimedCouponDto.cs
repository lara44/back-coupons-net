using System;

namespace back_coupons.DTOs.Response;

public class ClaimedCouponDto
{
    public int Id { get; set; }
    public DateTime DateRedeem { get; set; }
    public CouponBasicDto Coupon { get; set; } = null!;
    public ClientBasicDto Client { get; set; } = null!;
}
