using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JBNG20250326.AppWebMVC.Models;

public partial class Brand
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El Nombre es obligatorio.")]
    [Display(Name = "Marca")]
    public string BrandName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
