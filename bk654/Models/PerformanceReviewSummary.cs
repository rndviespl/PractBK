using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class PerformanceReviewSummary
{
    public int WorkerId { get; set; }

    public string? FullName { get; set; }

    public decimal? AvgRating { get; set; }

    public decimal? MaxRating { get; set; }

    public decimal? MinRating { get; set; }
}
