using System;
using System.Collections.Generic;

namespace SPC_Faculty_Pages.Models.Database;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
}
