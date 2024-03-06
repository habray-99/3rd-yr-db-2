using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using roider.Models;

namespace roider.Controllers
{
    public class LessonsController : Controller
    {
        private readonly Lessons _lessonsModel = new();

        // GET: Lessons
        public IActionResult Index()
        {
            List<Lessons> lessonsList = _lessonsModel.FetchLessons();
            return View(lessonsList);
        }

        // GET: Lessons/Create
        public IActionResult Create()
        {
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
            if (id == 0)
            {
                return NotFound();
            }

            var lesson = _lessonsModel.FetchLessonById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Lessons lesson)
        {
            if (id != lesson.LessonID)
            {
                return NotFound();
            }

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
            if (id == 0)
            {
                return NotFound();
            }

            var lesson = _lessonsModel.FetchLessonById(id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _lessonsModel.DeleteLesson(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Lessons/Details/5
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var lesson = _lessonsModel.FetchLessonById(id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }
    }
}
