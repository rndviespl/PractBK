using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class Restaurant
{
    public int RestaurantId { get; set; }

    public string RestaurantCode { get; set; } = null!;

    public string? Town { get; set; }

    public string Address { get; set; } = null!;

    public string? Mall { get; set; }

    public int? WorkersCount { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
