using System;

namespace back_coupons.DTOs.Response;

public class ClaimedCouponClientDto
{
    public int Id { get; set; }
    public DateTime DateRedeem { get; set; }
    public CouponClientDto Coupon { get; set; } = null!;
    public ClientBasicDto Client { get; set; } = null!;

    public string? Url { get; set; }
}
