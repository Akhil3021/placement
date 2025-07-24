using System;
using System.Collections.Generic;

namespace placement.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Query> Queries { get; set; } = new List<Query>();

    public virtual ICollection<QueryResponse> QueryResponses { get; set; } = new List<QueryResponse>();

    public virtual ICollection<Task> TaskAssignedByNavigations { get; set; } = new List<Task>();

    public virtual ICollection<Task> TaskAssignedToNavigations { get; set; } = new List<Task>();
}
