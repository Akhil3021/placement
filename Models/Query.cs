using System;
using System.Collections.Generic;

namespace placement.Models;

public partial class Query
{
    public int Qid { get; set; }

    public int? TaskId { get; set; }

    public int? RaisedBy { get; set; }

    public string? Subject { get; set; }

    public string? Description { get; set; }

    public string? Attachement { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<QueryResponse> QueryResponses { get; set; } = new List<QueryResponse>();

    public virtual User? RaisedByNavigation { get; set; }

    public virtual Task? Task { get; set; }
}
