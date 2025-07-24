using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace placement.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Query> Queries { get; set; } = new List<Query>();
    [JsonIgnore]
    public virtual ICollection<QueryResponse> QueryResponses { get; set; } = new List<QueryResponse>();
    [JsonIgnore]
    public virtual ICollection<Task> TaskAssignedByNavigations { get; set; } = new List<Task>();

    public virtual ICollection<Task> TaskAssignedToNavigations { get; set; } = new List<Task>();
}
