using System;

namespace back_coupons.DTOs.Response;

public class CompanyRedemptionCountDto
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public string Nit { get; set; } = null!;
    public int RedemptionCount { get; set; }
}
