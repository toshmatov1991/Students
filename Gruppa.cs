using System;
using System.Collections.Generic;

namespace Students;

public partial class Gruppa
{
    public long Id { get; set; }

    public long? NumberGroup { get; set; }

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
