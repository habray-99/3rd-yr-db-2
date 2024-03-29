﻿using Microsoft.AspNetCore.Mvc;
using roider.Models;

namespace roider.Controllers;

public class LessonsController : Controller
{
    private readonly Lessons _lessonsModel = new();

    // GET: Lessons
    public IActionResult Index()
    {
        var lessonsList = _lessonsModel.FetchLessons();
        return View(lessonsList);
    }

    // GET: Lessons/Create
    public IActionResult Create()
    {
        var courses = new Courses().FetchCourses();
        ViewBag.CoursesList = courses;
        return View();
    }

    // POST: Lessons/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Lessons lesson)
    {
        if (ModelState.IsValid)
        {
            _lessonsModel.AddLesson(lesson);
            return RedirectToAction(nameof(Index));
        }

        return View(lesson);
    }

    // GET: Lessons/Edit/5
    public IActionResult Edit(int id)
    {
        if (id == 0) return NotFound();

        var lesson = _lessonsModel.FetchLessonById(id);
        if (lesson == null) return NotFound();
        var courses = new Courses().FetchCourses();
        ViewBag.CoursesList = courses;

        return View(lesson);
    }

    // POST: Lessons/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Lessons lesson)
    {
        if (id != lesson.LessonId) return NotFound();

        if (ModelState.IsValid)
        {
            _lessonsModel.EditLesson(lesson, id);
            return RedirectToAction(nameof(Index));
        }

        return View(lesson);
    }

    // GET: Lessons/Delete/5
    public IActionResult Delete(int id)
    {
        if (id == 0) return NotFound();

        var lesson = _lessonsModel.FetchLessonById(id);
        if (lesson == null) return NotFound();

        return View(lesson);
    }

    // POST: Lessons/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _lessonsModel.DeleteLesson(id);
        return RedirectToAction(nameof(Index));
    }

    // GET: Lessons/Details/5
    public IActionResult Details(int id)
    {
        if (id == 0) return NotFound();

        var lesson = _lessonsModel.FetchLessonById(id);
        if (lesson == null) return NotFound();

        return View(lesson);
    }

    public async Task<IActionResult> SearchLessons(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return NotFound();

        var lessons = await _lessonsModel.SearchLessonsAsync(searchTerm);

        if (lessons == null) return NotFound();

        return PartialView("_LessonList", lessons);
    }
}