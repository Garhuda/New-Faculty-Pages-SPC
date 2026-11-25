namespace SPC_Faculty_Pages.Models;

public class FacultyInformation
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
