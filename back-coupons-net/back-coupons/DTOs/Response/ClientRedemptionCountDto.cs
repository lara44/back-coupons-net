
namespace back_coupons.DTOs.Response;

public class ClientRedemptionCountDto
{
    public int ClientId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int RedemptionCount { get; set; }
}
