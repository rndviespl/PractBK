using System;
using System.Collections.Generic;

namespace bk654.Models;

public partial class WorkersInfoView
{
    public int WorkerId { get; set; }

    public string? FullName { get; set; }

    public DateTime StartDate { get; set; }

    public string Position { get; set; } = null!;

    public string? RestaurantInfo { get; set; }
}
