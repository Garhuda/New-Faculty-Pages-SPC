using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using SPC_Faculty_Pages.Models;
using MySqlConnector;


namespace SPC_Faculty_Pages.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult FacultyProfile()
    {
        return View("faculty_profile");
    }

    public IActionResult FacultyInformationCollection()
    {
        return View("faculty_information_collection");
    }

    public async Task<IActionResult> TestDatabase()
    {
        var connectionString =
            _configuration.GetConnectionString("SPC-Faculty-Pages")
            ?? throw new InvalidOperationException("MySQL connection string is missing.");

        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        await using var command = new MySqlCommand("SELECT 1", connection);
        var result = await command.ExecuteScalarAsync();

        return Content($"Connected; SELECT 1 returned {result}.");
    }
}
