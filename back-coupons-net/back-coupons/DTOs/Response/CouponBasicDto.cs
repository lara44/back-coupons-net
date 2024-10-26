using System;

namespace back_coupons.DTOs.Response;

public class CouponBasicDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string CouponCode { get; set; } = null!;
}
