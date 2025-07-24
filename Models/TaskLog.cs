using System;
using System.Collections.Generic;

namespace placement.Models;

public partial class TaskLog
{
    public int Lid { get; set; }

    public int? EmpId { get; set; }

    public int? TaskId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public TimeOnly? BreakTime { get; set; }

    public TimeOnly? CompleteTime { get; set; }

    public string? Reason { get; set; }

    public virtual Task? Task { get; set; }
}
