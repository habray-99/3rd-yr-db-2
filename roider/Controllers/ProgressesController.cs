using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using roider.Models;

namespace roider.Controllers
{
    public class ProgressesController : Controller
    {
        private readonly Progresses _progressesModel;

        public ProgressesController()
        {
            _progressesModel = new Progresses();
        }

        // GET: Progresses
        public IActionResult Index()
        {
            List<Progresses> progressesList = _progressesModel.FetchProgresses();
            return View(progressesList);
        }

        // GET: Progresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Progresses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Progresses progress)
        {
            if (ModelState.IsValid)
            {
                _progressesModel.AddProgress(progress);
                return RedirectToAction(nameof(Index));
            }
            return View(progress);
        }

        // GET: Progresses/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var progress = _progressesModel.FetchProgressById(id);
            if (progress == null)
            {
                return NotFound();
            }
            return View(progress);
        }

        // POST: Progresses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Progresses progress)
        {
            if (id != progress.ProgressID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _progressesModel.EditProgress(progress, id);
                return RedirectToAction(nameof(Index));
            }
            return View(progress);
        }

        // GET: Progresses/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var progress = _progressesModel.FetchProgressById(id);
            if (progress == null)
            {
                return NotFound();
            }

            return View(progress);
        }

        // POST: Progresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _progressesModel.DeleteProgress(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Progresses/Details/5
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var progress = _progressesModel.FetchProgressById(id);
            if (progress == null)
            {
                return NotFound();
            }

            return View(progress);
        }
    }
}
