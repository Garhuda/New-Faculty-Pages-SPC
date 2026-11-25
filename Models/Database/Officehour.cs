using System;
using System.Collections.Generic;

namespace SPC_Faculty_Pages.Models.Database;

public partial class Officehour
{
    public int OfficeHourId { get; set; }

    public int FacultyId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public string? Location { get; set; }

    public virtual Faculty Faculty { get; set; } = null!;
}
