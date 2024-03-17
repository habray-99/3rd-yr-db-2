using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using roider.Models;

namespace roider.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly Instructors _instructorsModel = new();

        // GET: Instructors
        public IActionResult Index()
        {
            List<Instructors> instructorsList = _instructorsModel.FetchInstructors();
            return View(instructorsList);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Instructors instructor)
        {
            if (ModelState.IsValid)
            {
                _instructorsModel.AddInstructor(instructor);
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null) // Corrected condition
            {
                return NotFound();
            }

            var instructor = _instructorsModel.FetchInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Instructors instructor)
        {
            if (id != instructor.InstructorId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(instructor);
            _instructorsModel.EditInstructor(instructor, id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Instructors/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null) // Corrected condition
            {
                return NotFound();
            }

            var instructor = _instructorsModel.FetchInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _instructorsModel.DeleteInstructor(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Instructors/Details/5
        public IActionResult Details(string? id)
        {
            if (id == null) // Corrected condition
            {
                return NotFound();
            }

            var instructor = _instructorsModel.FetchInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }
    public async Task<IActionResult> SearchInstructors(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return NotFound();
        }

        var instructors = await _instructorsModel.SearchInstructorsAsync(searchTerm);

        if (instructors == null)
        {
            return NotFound();
        }

        return PartialView("_InstructorList", instructors);
    }
    }


}
