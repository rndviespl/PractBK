using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class DeletedWorker
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

    public DateTime? DeletedTimestamp { get; set; }

    public virtual PositionAtWork Position { get; set; } = null!;

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
