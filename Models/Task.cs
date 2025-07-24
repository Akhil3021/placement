using System;
using System.Collections.Generic;

namespace placement.Models;

public partial class Task
{
    public int Tid { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? EstimatedHours { get; set; }

    public int? AssignedTo { get; set; }

    public int? AssignedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? AssignedByNavigation { get; set; }

    public virtual User? AssignedToNavigation { get; set; }

    public virtual ICollection<Query> Queries { get; set; } = new List<Query>();

    public virtual ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();
}
