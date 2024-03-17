using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using roider.Datas;
using roider.Models;
using System.Collections.Generic;

namespace roider.Controllers
{
    public class CoursesController : Controller
    {
        private readonly Courses _coursesModel = new();

        // GET: Courses
        public IActionResult Index()
        {
            List<Courses> coursesList = _coursesModel.FetchCourses();
            return View(coursesList);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            var instructors = new Instructors().FetchInstructors();
            ViewBag.InstructorList = instructors;
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Courses course)
        {
            if (!ModelState.IsValid) return View(course);
            _coursesModel.AddCourse(course);
            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            var course = _coursesModel.FetchCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            var instructors = new Instructors().FetchInstructors();
            ViewBag.InstructorList = instructors;
            return View(course);
        }
        //
        // // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Courses course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }
        
            if (ModelState.IsValid)
            {
                _coursesModel.EditCourse(course, id);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            var course = _coursesModel.FetchCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
        
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _coursesModel.DeleteCourse(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Details/5
        // public IActionResult Details(string id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var course = _coursesModel.FetchCourseById(id);
        //     if (course == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(course);
        // }
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _coursesModel.FetchCourseDetailsAndInstructors(id);
            if (course == null)
            {
                return NotFound();
            }
            var lessons = new Lessons().FetchLessonsByCourse(id);
            ViewBag.LessonList = lessons;
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> SearchCourses(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return NotFound();
            }

            var courses = await _coursesModel.SearchCoursesAsync(searchTerm);

            if (courses == null)
            {
                return NotFound();
            }

            return PartialView("_CourseList", courses);
        }

    }

}
