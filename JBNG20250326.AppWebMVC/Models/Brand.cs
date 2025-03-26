﻿using System;
using System.Collections.Generic;

namespace JBNG20250326.AppWebMVC.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string BrandName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
