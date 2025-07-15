using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
=======
>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternalTraining.Areas.AdminCompany.Controllers
{
    [Area("AdminCompany")]
<<<<<<< HEAD
    [Authorize(Roles = "Company")]
=======
>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

<<<<<<< HEAD
        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
=======
        public HomeController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowCourse()
        {
            var currentUser = userManager.GetUserId(User);
<<<<<<< HEAD
            var myCourses = unitOfWork.BookingCourses.Get(e => e.CompanyUserId == currentUser, includes: [e => e.Course, e => e.Course.Chapters]).ToList();
            return View(myCourses);
=======
            var myCourses = unitOfWork.BookingCourses.Get(e=>e.CompanyUserId == currentUser,includes: [e=>e.Course, e=>e.Course.Chapters]).ToList();
            return View(myCourses);   
>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5
        }

        public IActionResult CourseDetails(int id)
        {
            var currentUser = userManager.GetUserId(User);
<<<<<<< HEAD

            var myCourse = unitOfWork.BookingCourses.Get(
                e => e.CompanyUserId == currentUser && e.CourseId == id,
                includes: [e => e.Course, e => e.Course.Chapters]
            ).FirstOrDefault();

            if (myCourse?.Course?.Chapters != null)
            {
                foreach (var chapter in myCourse.Course.Chapters)
                {
                    chapter.Lessons = unitOfWork.Lessons.Get(l => l.ChapterId == chapter.Id).ToList();
                }
            }

=======
            var myCourse = unitOfWork.BookingCourses.Get(e => e.CompanyUserId == currentUser && e.CourseId == id, includes: [e => e.Course, e => e.Course.Chapters]).ToList();
>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5
            return View(myCourse);
        }
    }
}
