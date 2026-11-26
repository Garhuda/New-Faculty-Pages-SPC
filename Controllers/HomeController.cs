using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPC_Faculty_Pages.Data;
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

    public async Task<IActionResult> FacultyProfile()
    {
        var faculty = await _db.Faculties.AsNoTracking().ToListAsync();
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

    public IActionResult FacultyInformationCollection()
    {
        return View("faculty_information_collection");
    }
}
