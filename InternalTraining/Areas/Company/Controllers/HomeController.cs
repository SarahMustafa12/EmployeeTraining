using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace InternalTraining.Areas.Company.Controllers
{
    [Area ("Company")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var courses = _unitOfWork.Courses.Get().Take(3).ToList();
            return View(courses);
        }
        public IActionResult Courses()
        {
            var courses = _unitOfWork.Courses.Get().ToList();
            return View(courses);
        }
        public IActionResult Details(int id)
        {
            var course = _unitOfWork.Courses.GetOne(c => c.Id == id, includes: [e => e.Chapters, e =>e.Lessons]);
            if (course == null)
            {
                return RedirectToAction("Error", "Courses");
            }
            return View(course);
        }

        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult NotFoundPage()
        { 
            return View();
        }
    }
}
