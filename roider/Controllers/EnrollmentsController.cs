using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using roider.Models;

namespace roider.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly Enrollments _enrollmentsModel = new();

        // GET: Enrollments
        public IActionResult Index()
        {
            List<Enrollments> enrollmentsList = _enrollmentsModel.GetEnrollments();
            return View(enrollmentsList);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Enrollments enrollment)
        {
            Console.WriteLine(ModelState.IsValid);
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _enrollmentsModel.AddEnrollment(enrollment);
                return RedirectToAction(nameof(Index));
            }
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var enrollment = _enrollmentsModel.FetchEnrollmentsByStudentId(id).Find(e => e.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Enrollments enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _enrollmentsModel.EditEnrollment(enrollment, id);
                return RedirectToAction(nameof(Index));
            }
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var enrollment = _enrollmentsModel.FetchEnrollmentsByStudentId(id).Find(e => e.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _enrollmentsModel.DeleteEnrollment(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Enrollments/Details/5
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var enrollment = _enrollmentsModel.FetchEnrollmentsByStudentId(id).Find(e => e.EnrollmentId == id);
            if (enrollment == null) { return NotFound(); }
            return View(enrollment);
        }
    }
}
