using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public virtual User? RaisedByNavigation { get; set; }
    [JsonIgnore]
    public virtual Task? Task { get; set; }
}
