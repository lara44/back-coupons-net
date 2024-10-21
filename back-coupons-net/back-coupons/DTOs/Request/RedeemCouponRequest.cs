using System.ComponentModel.DataAnnotations;

namespace back_coupons.DTOs.Request
{
    public class RedeemCouponRequest
    {
        [Display(Name = "Cupón")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CouponId { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ClientId { get; set; }

        [Display(Name = "Signature")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Signature { get; set; } = null!;
    }
}
