using System;
using System.Collections.Generic;

namespace SPC_Faculty_Pages.Models.Database;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseCode { get; set; } = null!;

    public string CourseTitle { get; set; } = null!;

    public string? SourceSystem { get; set; }

    public virtual ICollection<Facultycourse> Facultycourses { get; set; } = new List<Facultycourse>();
}
