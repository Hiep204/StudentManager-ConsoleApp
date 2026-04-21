using System;
using System.Collections.Generic;

namespace Ass2.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public virtual ICollection<StudentMark> StudentMarks { get; set; } = new List<StudentMark>();
}
