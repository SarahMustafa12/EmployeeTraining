using InternalTraining.Models;
using InternalTraining.Repositories;
using InternalTraining.Repositories.IRepository;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternalTraining.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LessonController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LessonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int page = 1 )
        {

            var lessons = _unitOfWork.Lessons.Get(includes: [e => e.Course, e => e.Chapter]);

            int totalCount = lessons.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (page > totalPages && totalPages > 0)
                return RedirectToAction("NotFoundPage", "Home", new { area = "End User" });

            lessons = lessons.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.totalPages = totalPages;
            return View(lessons.ToList());
        }
        [HttpGet]
        public IActionResult GetChaptersByCourse(int id)
        {
            var chapters = _unitOfWork.Chapters.Get(c => c.CourseId == id)
                .Select(c => new { id = c.Id, title = c.Name }).ToList();

            return Json(chapters);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var courses = _unitOfWork.Courses.Get(); // Get courses instead of chapters3
            ViewBag.CourseId = new SelectList(courses, "Id", "Name");

            var chapters = _unitOfWork.Chapters.Get();
            ViewBag.ChapterId = new SelectList(chapters, "Id", "Name");
            return View(new Lesson());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Lessons.Create(lesson);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            var courses = _unitOfWork.Courses.Get();
            var chapters = _unitOfWork.Chapters.Get();

            ViewBag.CourseId = new SelectList(courses, "Id", "Name", lesson.CourseId);
            ViewBag.ChapterId = new SelectList(chapters, "Id", "Title", lesson.ChapterId);

            return View(lesson);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var lesson = _unitOfWork.Lessons.GetOne(e => e.Id == id);
            if (lesson == null)
                return RedirectToAction("Error", "Lesson");

            var chapters = _unitOfWork.Chapters.Get();
            var courses = _unitOfWork.Courses.Get();
            ViewBag.CourseId = new SelectList(courses, "Id", "Name", lesson.CourseId);
            ViewBag.ChapterId = new SelectList(chapters, "Id", "Name", lesson.CourseId);
            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Lessons.Update(lesson);
                _unitOfWork.Commit();
                return RedirectToAction("Index", "Lesson");
            }

            var chapters = _unitOfWork.Chapters.Get();
            ViewBag.ChapterId = new SelectList(chapters, "Id", "Name", lesson.CourseId);
                        var courses = _unitOfWork.Courses.Get();
            ViewBag.CourseId = new SelectList(courses, "Id", "Name", lesson.CourseId);
            return View(lesson);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var lesson = _unitOfWork.Lessons.GetOne(e => e.Id == id);
            if (lesson == null)
                return RedirectToAction("Error", "Lesson");

            _unitOfWork.Lessons.Delete(lesson);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
