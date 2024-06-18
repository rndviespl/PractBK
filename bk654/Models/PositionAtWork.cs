using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class PositionAtWork
{
    public int PositionId { get; set; }

    public string Name { get; set; } = null!;

    public decimal SalaryPerHour { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
