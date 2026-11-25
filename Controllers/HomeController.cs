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

    public async Task<IActionResult> FacultyList()
    {
         var faculty = await _db.Faculties.AsNoTracking().ToListAsync();
        return View("~/Views/ClearwaterCampus/FacultyList.cshtml", faculty);
    }

    public IActionResult FacultyInformationCollection()
    {
        return View("faculty_information_collection");
    }
}
