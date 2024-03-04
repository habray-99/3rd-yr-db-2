using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using roider.Models;

namespace roider.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly Instructors _instructorsModel;

        public InstructorsController()
        {
            _instructorsModel = new Instructors();
        }

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
        public IActionResult Edit(int id)
        {
            if (id == 0)
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
        public IActionResult Edit(int id, Instructors instructor)
        {
            if (id != instructor.InstructorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _instructorsModel.EditInstructor(instructor, id);
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == 0)
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
        public IActionResult DeleteConfirmed(int id)
        {
            _instructorsModel.DeleteInstructor(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Instructors/Details/5
        public IActionResult Details(int id)
        {
            if (id == 0)
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
    }
}
