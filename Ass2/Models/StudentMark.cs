using System;
using System.Collections.Generic;

namespace Ass2.Models;

public partial class StudentMark
{
    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public decimal Mark { get; set; }

    public DateOnly GradeDate { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
