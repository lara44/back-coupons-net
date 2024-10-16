using System.ComponentModel.DataAnnotations;

namespace back_coupons.DTOs.Request
{
    public class ClaimCouponRequest
    {
        [Display(Name = "Cupón")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Code { get; set; } = null!;

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ClientId { get; set; }
    }
}
