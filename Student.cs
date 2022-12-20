using System;
using System.Collections.Generic;

namespace Students;

public partial class Student
{
    public long Id { get; set; }

    public string? Firstname { get; set; }

    public string? Name { get; set; }

    public string? Lastname { get; set; }

    public string? Birthday { get; set; }

    public long? FkIdGroup { get; set; }

    public long? FkIdFakulity { get; set; }

    public virtual Fakulity? FkIdFakulityNavigation { get; set; }

    public virtual Gruppa? FkIdGroupNavigation { get; set; }
}
