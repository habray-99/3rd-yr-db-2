using Microsoft.AspNetCore.Mvc;
using roider.Models;

namespace roider.Controllers;

public class ProgressesController : Controller
{
    private readonly Progresses _progressesModel = new();

    // GET: Progresses
    public IActionResult Index()
    {
        var progressesList = _progressesModel.FetchProgressesAsync().Result;
        return View(progressesList);
    }

    // GET: Progresses/Create
    public IActionResult Create()
    {
        var students = new Students().GetStudents();
        var lessons = new Lessons().FetchLessons();
        var courses = new Courses().FetchCourses();
        ViewBag.StudentsList = students;
        ViewBag.LessonsList = lessons;
        ViewBag.CoursesList = courses;
        return View();
    }
    // GET: Progresses/Create
    // public async Task<IActionResult> Create()
    // {
    //     var students = await new Students().GetStudentsAsync();
    //     var lessons =  new Lessons().FetchLessons();
    //     var courses =  new Courses().FetchCourses();
    //     ViewBag.StudentsList = students;
    //     ViewBag.LessonsList = lessons;
    //     ViewBag.CoursesList = courses;
    //     return View();
    // }


    // POST: Progresses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Progresses progress)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    // Log the error message
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }
        }

        if (ModelState.IsValid)
        {
            _progressesModel.AddProgressAsync(progress);
            return RedirectToAction(nameof(Index));
        }

        return View(progress);
    }

    // GET: Progresses/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        if (id == 0) return NotFound();

        var progress = await _progressesModel.FetchProgressByIdAsync(id);
        if (progress == null) return NotFound();

        var students = new Students().GetStudents();
        var lessons = new Lessons().FetchLessons();
        var courses = new Courses().FetchCourses();
        ViewBag.CoursesList = courses;
        ViewBag.StudentsList = students;
        ViewBag.LessonsList = lessons;

        return View(progress);
    }

    // POST: Progresses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Progresses progress)
    {
        if (id != progress.ProgressId) return NotFound();

        if (ModelState.IsValid)
        {
            await _progressesModel.EditProgressAsync(progress);
            return RedirectToAction(nameof(Index));
        }

        return View(progress);
    }

    // GET: Progresses/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        if (id == 0) return NotFound();

        var progress = await _progressesModel.FetchProgressByIdAsync(id);
        if (progress == null) return NotFound();

        return View(progress);
    }

    // POST: Progresses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _progressesModel.DeleteProgressAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // GET: Progresses/Details/5
    public async Task<IActionResult> Details(int id)
    {
        if (id == 0) return NotFound();

        var progress = await _progressesModel.FetchProgressByIdAsync(id);
        if (progress == null) return NotFound();

        return View(progress);
    }

    public async Task<IActionResult> SearchLessons(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return NotFound();

        var progresses = await _progressesModel.SearchProgressesAsync(searchTerm);

        if (progresses == null) return NotFound();

        return PartialView("_ProgressList", progresses);
    }
}