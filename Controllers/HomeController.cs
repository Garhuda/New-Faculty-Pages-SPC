using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPC_Faculty_Pages.Data;
using SPC_Faculty_Pages.Models.Database;
using MySqlConnector;

namespace SPC_Faculty_Pages.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    private readonly FacultyDbContext _db;
    public HomeController(IConfiguration configuration, FacultyDbContext db)
    {
        _configuration = configuration;
        _db = db;
    }

    public async Task<IActionResult> FacultyProfile(int? id)
    {
        var faculty = await _db.Faculties
            .AsNoTracking()
            .Include(f => f.Department)
            .Include(f => f.Facultycourses)
                .ThenInclude(fc => fc.Course)
            .FirstOrDefaultAsync(f => id == null || f.FacultyId == id);

        if (faculty == null)
        {
            return NotFound();
        }

        return View("faculty_profile", faculty);
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> FacultyListASC()
    {
        var faculty = await _db.Faculties.AsNoTracking()
            .Where(f => f.OfficeLocation == "All State Center")
            .ToListAsync();
        return View("~/Views/CampusFaculty/FacultyListASC.cshtml", faculty);
    }

    public async Task<IActionResult> FacultyListCL()
    {
        var faculty = await _db.Faculties.AsNoTracking()
            .Where(f => f.OfficeLocation == "Clearwater")
            .ToListAsync();
        return View("~/Views/CampusFaculty/FacultyListCL.cshtml", faculty);
    }

    public async Task<IActionResult> FacultyListDT()
    {
        var faculty = await _db.Faculties.AsNoTracking()
            .Where(f => f.OfficeLocation == "Downtown")
            .ToListAsync();
        return View("~/Views/CampusFaculty/FacultyListDT.cshtml", faculty);
    }

    public async Task<IActionResult> FacultyListHEC()
    {
        var faculty = await _db.Faculties.AsNoTracking()
            .Where(f => f.OfficeLocation == "Health Education Center")
            .ToListAsync();
        return View("~/Views/CampusFaculty/FacultyListHEC.cshtml", faculty);
    }

    public async Task<IActionResult> FacultyListSC()
    {
        var faculty = await _db.Faculties.AsNoTracking()
            .Where(f => f.OfficeLocation == "Seminole Campus")
            .ToListAsync();
        return View("~/Views/CampusFaculty/FacultyListSC.cshtml", faculty);
    }

    public async Task<IActionResult> FacultyListEP()
    {
        var faculty = await _db.Faculties.AsNoTracking()
            .Where(f => f.OfficeLocation == "Epicenter")
            .ToListAsync();
        return View("~/Views/CampusFaculty/FacultyListEP.cshtml", faculty);
    }

    public async Task<IActionResult> FacultyListGB()
    {
        var faculty = await _db.Faculties.AsNoTracking()
            .Where(f => f.OfficeLocation == "Gibbs Campus")
            .ToListAsync();
        return View("~/Views/CampusFaculty/FacultyListGB.cshtml", faculty);
    }

    public async Task<IActionResult> FacultyListTP()
    {
        var faculty = await _db.Faculties.AsNoTracking()
            .Where(f => f.OfficeLocation == "Tarpon Campus")
            .ToListAsync();
        return View("~/Views/CampusFaculty/FacultyListTP.cshtml", faculty);
    }

    public async Task<IActionResult> FacultyInformationCollection(int? id, string? campus)
    {
        var departments = await _db.Departments.AsNoTracking()
            .OrderBy(d => d.DepartmentName)
            .ToListAsync();
        var courses = await _db.Courses.AsNoTracking()
            .OrderBy(c => c.CourseCode)
            .ToListAsync();
        var facultyList = await _db.Faculties.AsNoTracking()
            .Include(f => f.Department)
            .OrderBy(f => f.LastName)
            .ThenBy(f => f.FirstName)
            .ToListAsync();

        Faculty? selectedFaculty = null;
        if (id.GetValueOrDefault() > 0)
        {
            selectedFaculty = await _db.Faculties
                .Include(f => f.Department)
                .Include(f => f.Facultycourses)
                    .ThenInclude(fc => fc.Course)
                .FirstOrDefaultAsync(f => f.FacultyId == id);
        }

        if (selectedFaculty == null && id != 0)
        {
            selectedFaculty = facultyList.FirstOrDefault();
        }

        ViewBag.Departments = departments;
        ViewBag.Courses = courses;
        ViewBag.FacultyList = facultyList;

        var facultyToEdit = selectedFaculty ?? new Faculty
        {
            OfficeLocation = id == 0 ? campus : null
        };

        return View("faculty_information_collection", facultyToEdit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveFaculty(Faculty faculty)
    {
        if (string.IsNullOrWhiteSpace(faculty.FirstName) ||
            string.IsNullOrWhiteSpace(faculty.LastName) ||
            string.IsNullOrWhiteSpace(faculty.Email) ||
            faculty.DepartmentId == 0)
        {
            TempData["Error"] = "First name, last name, email, and department are required.";
            return RedirectToAction(nameof(FacultyInformationCollection),
                new { id = faculty.FacultyId > 0 ? faculty.FacultyId : (int?)null });
        }

        faculty.FirstName = faculty.FirstName.Trim();
        faculty.LastName = faculty.LastName.Trim();
        faculty.Email = faculty.Email.Trim();

        if (faculty.FacultyId == 0)
        {
            _db.Faculties.Add(faculty);
        }
        else
        {
            var existing = await _db.Faculties.FindAsync(faculty.FacultyId);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DepartmentId = faculty.DepartmentId;
            existing.FirstName = faculty.FirstName;
            existing.LastName = faculty.LastName;
            existing.Email = faculty.Email;
            existing.Phone = faculty.Phone;
            existing.OfficeLocation = faculty.OfficeLocation;
            existing.Biography = faculty.Biography;
        }

        try
        {
            await _db.SaveChangesAsync();
            TempData["Success"] = faculty.FacultyId == 0
                ? "Faculty record created."
                : "Faculty record updated.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Unable to save the faculty record. Check that the email is unique.";
        }

        var savedId = faculty.FacultyId;
        return RedirectToAction(nameof(FacultyInformationCollection), new { id = savedId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFaculty(int facultyId)
    {
        var faculty = await _db.Faculties.FindAsync(facultyId);
        if (faculty == null)
        {
            return NotFound();
        }

        var courses = await _db.Facultycourses.Where(fc => fc.FacultyId == facultyId).ToListAsync();
        var officeHours = await _db.Officehours.Where(oh => oh.FacultyId == facultyId).ToListAsync();
        _db.Facultycourses.RemoveRange(courses);
        _db.Officehours.RemoveRange(officeHours);
        _db.Faculties.Remove(faculty);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Faculty record deleted.";
        return RedirectToAction(nameof(FacultyInformationCollection), new { id = 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFacultyCourse(int facultyId, int courseId, string? term)
    {
        if (facultyId <= 0 || courseId <= 0)
        {
            TempData["Error"] = "Select a faculty member and course.";
            return RedirectToAction(nameof(FacultyInformationCollection), new { id = facultyId });
        }

        var exists = await _db.Facultycourses
            .AnyAsync(fc => fc.FacultyId == facultyId && fc.CourseId == courseId);
        if (!exists)
        {
            _db.Facultycourses.Add(new Facultycourse
            {
                FacultyId = facultyId,
                CourseId = courseId,
                Term = string.IsNullOrWhiteSpace(term) ? null : term.Trim()
            });
            await _db.SaveChangesAsync();
            TempData["Success"] = "Course added.";
        }
        else
        {
            TempData["Error"] = "That course is already assigned to this faculty member.";
        }

        return RedirectToAction(nameof(FacultyInformationCollection), new { id = facultyId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFacultyCourse(int facultyId, int courseId)
    {
        var enrollment = await _db.Facultycourses
            .FirstOrDefaultAsync(fc => fc.FacultyId == facultyId && fc.CourseId == courseId);
        if (enrollment != null)
        {
            _db.Facultycourses.Remove(enrollment);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Course removed.";
        }

        return RedirectToAction(nameof(FacultyInformationCollection), new { id = facultyId });
    }
}
