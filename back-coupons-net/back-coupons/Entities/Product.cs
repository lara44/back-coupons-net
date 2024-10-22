using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_coupons.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Nombre producto")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

        [Display(Name = "Código de barra")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Barcode { get; set; } = null!;

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        public ICollection<DetailCoupon> DetailCoupons { get; set; }
    }
}
