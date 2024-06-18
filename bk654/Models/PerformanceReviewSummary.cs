using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class PerformanceReviewSummary
{
    public int WorkerId { get; set; }

    public decimal? AvgRating { get; set; }

    public int? MaxRating { get; set; }

    public int? MinRating { get; set; }
}
