using System.ComponentModel.DataAnnotations;

namespace back_coupons.Entities
{
    public class Company
    {
        public int Id { get; set; }

        [Display(Name = "Nit compañia")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nit { get; set; } = null!;

        [Display(Name = "Nombre compañia")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Dirección compañia")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; } = null!;

        [Display(Name = "Email compañia")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Telefono compañia")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Phone { get; set; } = null!;
        public ICollection<Contact>? Contacts { get; set; }
        public ICollection<Coupon>? Coupons { get; set; }
        public ICollection<User>? Users { get; set; } 
        public ICollection<Company>? Companies { get; set; } 
    }
}
