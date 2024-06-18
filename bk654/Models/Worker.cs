using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public int PositionId { get; set; }

    public int RestaurantId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? DismissalReason { get; set; }

    public virtual ICollection<PerformanceReview> PerformanceReviews { get; set; } = new List<PerformanceReview>();

    public virtual PositionAtWork Position { get; set; } = null!;

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual ICollection<WorkShift> WorkShifts { get; set; } = new List<WorkShift>();
}
