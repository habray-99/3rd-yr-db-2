﻿using Microsoft.AspNetCore.Mvc;
using roider.Models;

namespace roider.Controllers;

public class StudentsController : Controller
{
    private readonly Students _studentsModel = new();

    // GET: Students
    public IActionResult Index()
    {
        var studentsList = _studentsModel.FetchStudents();
        return View(studentsList);
    }

    // GET: Students/Create
    public IActionResult Create()
    {
        var CountriesList = new Country().GetCountries();
        ViewBag.CountriesList = CountriesList; // Changed to "CountriesList"
        return View();
    }


    // POST: Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Students student)
    {
        if (ModelState.IsValid)
        {
            _studentsModel.AddStudent(student);
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    // GET: Students/Edit/5
    public IActionResult Edit(int id)
    {
        if (id == 0) return NotFound();

        var student = _studentsModel.FetchStudentById(id);
        if (student == null) return NotFound();

        return View(student);
    }

    // POST: Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Students student)
    {
        if (id != student.StudentId) return NotFound();

        if (ModelState.IsValid)
        {
            _studentsModel.EditStudentDetails(student, id);
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    // GET: Students/Delete/5
    public IActionResult Delete(int id)
    {
        if (id == 0) return NotFound();

        var student = _studentsModel.FetchStudentById(id);
        if (student == null) return NotFound();

        return View(student);
    }

    // POST: Students/Delete/5
    // [HttpPost, ActionName("Delete")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _studentsModel.DeleteStudent(id);
        return RedirectToAction(nameof(Index));
    }

    // GET: Students/Details/5
    public async Task<IActionResult> Details(int id)
    {
        if (id == 0) return NotFound();

        var student = _studentsModel.FetchStudentById(id);
        if (student == null) return NotFound();

        var studentCourseProgress = new StudentCourseProgress();
        // Load student progress
        student.Progresses = await studentCourseProgress.FetchStudentCourseProgressAsync(id);

        return View(student);
    }

    public async Task<IActionResult> SearchStudents(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return NotFound();

        var students = await _studentsModel.SearchStudentsAsync(searchTerm);

        if (students == null) return NotFound();

        return PartialView("_StudentList", students);
    }
}