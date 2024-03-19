using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using roider.Models;

namespace roider.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    // public IActionResult Index()
    // {
    //     var currentYear = DateTime.Now.Year;
    //     var currentMonth = DateTime.Now.Month;
    //     var courses = new Courses();
    //     var topCourses = courses.GetTop3CoursesByEnrollment(currentYear, currentMonth);
    //     return View(topCourses);
    // }
    public IActionResult Index(string? date)
    {
        DateTime selectedDate;
        if (string.IsNullOrEmpty(date))
            selectedDate = DateTime.Now;
        else
            selectedDate = DateTime.ParseExact(date, "yyyy-MM", CultureInfo.InvariantCulture);

        var courses = new Courses();
        var topCourses = courses.GetTop3CoursesByEnrollment(selectedDate);
        ViewBag.SelectedDate = selectedDate; // Pass the selected date to the view
        return View(topCourses);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}