﻿using System.ComponentModel.DataAnnotations;

namespace back_coupons.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Nombre categoria")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
    }
}
