using InternalTraining.Models;
using InternalTraining.Repositories;
using InternalTraining.Repositories.IRepository;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternalTraining.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var allCourses = _unitOfWork.Courses.Get(); 

            int totalCourses = allCourses.Count();
            int totalPages = (int)Math.Ceiling((double)totalCourses / pageSize);

            var courses = allCourses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(courses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var courses = _unitOfWork.Courses.Get();
            
            ViewData["Courses"] = courses.ToList();

            return View(new Course());
        }

        [HttpPost]
        public IActionResult Create(Course course, IFormFile? file)
        {
            // Validation
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Save img in wwwroot
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\admin", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save img name in db
                    course.CourseImage = fileName;
                }

                _unitOfWork.Courses.Create(course);
                _unitOfWork.Commit();

               // return RedirectToAction("Index");
                return RedirectToAction("Index", "Course", new { area = "admin" });
            }
            return View(course);
        }

        [HttpGet]
        public IActionResult Edit(int courseId)
        {
            var course = _unitOfWork.Courses.GetOne(e => e.Id == courseId);

            if (course != null)
            {
                return View(course);
            }

            return RedirectToAction("Error", "Courses");
        }

        [HttpPost]
        public IActionResult Edit(Course course, IFormFile? file)
        {
            var courseInDb = _unitOfWork.Courses.GetOne(e => e.Id == course.Id, tracked: false);

            if (courseInDb != null && file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\admin", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Delete old img from wwwroot
            

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\admin", courseInDb.CourseImage);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Save img name in db
                course.CourseImage = fileName;
            }
            else
                course.CourseImage = courseInDb.CourseImage;

            if (course != null)
            {
               _unitOfWork.Courses.Update(course);
               _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Error", "Home");
        }

        public IActionResult DeleteImg(int courseId)
        {
            var course = _unitOfWork.Courses.GetOne(e => e.Id == courseId);

            if (course != null)
            {
                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\admin", course.CourseImage);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Delete img name in db
                course.CourseImage = null;
                _unitOfWork.Commit();

                return RedirectToAction("Edit","Course" ,new { courseId });
              
            }

            return RedirectToAction("Error", "Home");
        }

        public IActionResult Delete(int courseId)
        {
            var course = _unitOfWork.Courses.GetOne(e => e.Id == courseId);

            if (course != null)
            {
                // Delete old img from wwwroot
                if (course.CourseImage != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\admin", course.CourseImage);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                // Delete img name in db
                _unitOfWork.Courses.Delete(course);
                _unitOfWork.Commit();

                return RedirectToAction("Index", "Course", new { area = "admin" });
            }

            return RedirectToAction("Error", "Home");
        }

    }
}