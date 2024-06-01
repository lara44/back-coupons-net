using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_coupons.Entities
{
    public class Coupon
    {
        public int Id { get; set; }

        [Display(Name = "Nombre coupon")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Fecha inicio cupón")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha expiración cupón")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Cantidad Inicial")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int QuantityInitial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int QuantityActual { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [Display(Name = "Código Cupón")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string CouponCode { get; set; } = null!;

        public ICollection<DetailCoupon> DetailCoupons { get; set; }
    }
}
