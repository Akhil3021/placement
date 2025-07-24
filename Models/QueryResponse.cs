using System;
using System.Collections.Generic;

namespace placement.Models;

public partial class QueryResponse
{
    public int Qrid { get; set; }

    public int? QueryId { get; set; }

    public int? RespondedBy { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Query? Query { get; set; }

    public virtual User? RespondedByNavigation { get; set; }
}
