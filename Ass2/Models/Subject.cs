using System;
using System.Collections.Generic;

namespace Ass2.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<StudentMark> StudentMarks { get; set; } = new List<StudentMark>();
}
