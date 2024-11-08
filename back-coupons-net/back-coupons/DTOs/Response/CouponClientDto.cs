using System;
using back_coupons.Entities;

namespace back_coupons.DTOs.Response;

public class CouponClientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string CouponCode { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string? Photo { get; set; }
    public string? NameCompany {get; set; }
    public Enums.RedeemState? State { get; set; }
    public ICollection<DetailCouponDto>? DetailCoupons { get; set; }
}
