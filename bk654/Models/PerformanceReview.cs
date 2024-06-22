using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class PerformanceReview
{
    public int ReviewId { get; set; }

    public int WorkerId { get; set; }

    public string ReviewerName { get; set; } = null!;

    public DateTime ReviewDate { get; set; }

    public decimal PerformanceRating { get; set; }

    public string Comments { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
