using System;
using System.Collections.Generic;

namespace SPC_Faculty_Pages.Models.Database;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public int DepartmentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? OfficeLocation { get; set; }

    public string? Biography { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Facultycourse> Facultycourses { get; set; } = new List<Facultycourse>();

    public virtual ICollection<Officehour> Officehours { get; set; } = new List<Officehour>();
}
