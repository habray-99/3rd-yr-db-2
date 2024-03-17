using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using roider.Models;

namespace roider.Controllers
{
    public class QAsController : Controller
    {
        private readonly QAs _qasModel = new();

        // GET: QAs
        public IActionResult Index()
        {
            List<QAs> qasList = _qasModel.FetchQAs();
            return View(qasList);
        }

        // GET: QAs/Create
        public IActionResult Create()
        {
            var courses = new Courses().FetchCourses();
            ViewBag.CoursesList = courses;
            var students = new Students().GetStudents();
            ViewBag.StudentsList = students;
            return View();
        }

        // POST: QAs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QAs qa)
        {
            if (ModelState.IsValid)
            {
                _qasModel.AddQa(qa);
                return RedirectToAction(nameof(Index));
            }
            return View(qa);
        }

        // GET: QAs/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var qa = _qasModel.FetchQaById(id);
            if (qa == null)
            {
                return NotFound();
            }
            var courses = new Courses().FetchCourses();
            ViewBag.CoursesList = courses;
            var students = new Students().GetStudents();
            ViewBag.StudentsList = students;
            return View(qa);
        }

        // POST: QAs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, QAs qa)
        {
            if (id != qa.Qaid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _qasModel.EditQa(qa, id);
                return RedirectToAction(nameof(Index));
            }
            return View(qa);
        }

        // GET: QAs/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var qa = _qasModel.FetchQaById(id);
            if (qa == null)
            {
                return NotFound();
            }

            return View(qa);
        }

        // POST: QAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _qasModel.DeleteQa(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: QAs/Details/5
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var qa = _qasModel.FetchQaById(id);
            if (qa == null)
            {
                return NotFound();
            }

            return View(qa);
        }
        public async Task<IActionResult> SearchQAs(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return NotFound();
            }

            var qas = await _qasModel.SearchQAsAsync(searchTerm);

            if (qas == null)
            {
                return NotFound();
            }

            return PartialView("_QAsList", qas);
        }

    }
}
