using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class WorkShift
{
    public int WorkShiftId { get; set; }

    public int WorkerId { get; set; }

    public DateTime StartShift { get; set; }

    public DateTime EndShift { get; set; }

    public string? DescriptionManualEntry { get; set; }

    public virtual Worker Worker { get; set; } = null!;
}
