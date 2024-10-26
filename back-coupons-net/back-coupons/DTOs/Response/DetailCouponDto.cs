using System;

namespace back_coupons.DTOs.Response;

public class DetailCouponDto
{
    public int Id { get; set; }
    public int CouponId { get; set; }
    public ProductDto Product { get; set; } = null!;
}
