using InternalTraining.Models;
using InternalTraining.Repositories;
using InternalTraining.Repositories.IRepository;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace InternalTraining.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ChapterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IEnumerable courses;

        public ChapterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int page= 1)
        {
            var chapter = _unitOfWork.Chapters.Get(includes: [e => e.Course]);
            int totalCount = chapter.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (page > totalPages && totalPages > 0)
                return RedirectToAction("NotFoundPage", "Home", new { area = "End User" });

            chapter = chapter.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.totalPages = totalPages;
            return View(chapter.ToList());
        }

        [HttpGet]
        public IActionResult Create() 
        {

            var courses = _unitOfWork.Courses.Get(); 
            ViewBag.CourseId = new SelectList(courses, "Id", "Name");


            return View(new Chapter());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Chapters.Create(chapter);
                _unitOfWork.Commit();
                return RedirectToAction("Index", "Chapter", new { area = "Admin" });
            }

            var courses = _unitOfWork.Courses.Get();
            ViewBag.CourseId = new SelectList(courses, "Id", "Name", chapter.CourseId);
            return View(chapter);
        }

        // GET: Chapter/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var chapter = _unitOfWork.Chapters.GetOne(e => e.Id == id);
            if (chapter == null)
                return RedirectToAction("Error", "Home");

            var courses = _unitOfWork.Courses.Get();
            ViewData["CourseId"] = new SelectList(courses, "Id", "Name", chapter.CourseId);
            return View(chapter);
        }

        // POST: Chapter/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Chapters.Update(chapter);
                _unitOfWork.Commit();
                return RedirectToAction("Index", "Chapter", new { area = "Admin" });
            }

            var courses = _unitOfWork.Courses.Get();
            ViewData["CourseId"] = new SelectList(courses, "Id", "Name", chapter.CourseId);
            return View(chapter);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var chapter = _unitOfWork.Chapters.GetOne(e => e.Id == id);
            if (chapter == null)
                return RedirectToAction("Error", "Home");

            _unitOfWork.Chapters.Delete(chapter);
            _unitOfWork.Commit();

            return RedirectToAction("Index", "Course", new { area = "Admin" });
        }

    }
}
