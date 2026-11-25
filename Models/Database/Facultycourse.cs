using System;
using System.Collections.Generic;

namespace SPC_Faculty_Pages.Models.Database;

public partial class Facultycourse
{
    public int FacultyId { get; set; }

    public int CourseId { get; set; }

    public string? Term { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Faculty Faculty { get; set; } = null!;
}
