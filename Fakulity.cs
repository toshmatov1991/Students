using System;
using System.Collections.Generic;

namespace Students;

public partial class Fakulity
{
    public long Id { get; set; }

    public string? NameFaculity { get; set; }

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
