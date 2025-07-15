using InternalTraining.Models;
using InternalTraining.Test_Cookie;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InternalTraining.Areas.Company2.Controllers
{
    [Area("Company2")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
                
        }
        [AllowAnonymous]
        public IActionResult AccessViaToken(string token)
        {
            if (token != null)
            {
                var company = unitOfWork.ContactsUs.GetOne(e => e.Token == token);

                if (company != null)
                {
                    Response.Cookies.Append("ContactToken", company.Token);

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("AccessDenied", "Account", new { area = "Identity" });
        }

        [CookieOrRoleAuthorize("ContactToken", "Company")]
        public IActionResult Index()
        {
           
            var courses = unitOfWork.Courses.Get(includes: [e => e.Chapters, e => e.Lessons]);

            return View(courses.ToList());
        }

        [CookieOrRoleAuthorize("ContactToken", "Company")]
        public IActionResult Pricing()
        {

            return View();
        }

        [CookieOrRoleAuthorize("ContactToken", "Company")]
        public IActionResult Enroll(int id)
        {
            var token = Request.Cookies["ContactToken"];
            var appUserId = userManager.GetUserId(User);

            var getCompanyUser = unitOfWork.CompanyUsers.GetOne(e => e.Id == appUserId);
            var contact = unitOfWork.ContactsUs.GetOne(e =>
                (token != null && e.Token == token) ||
                (getCompanyUser != null && e.Id == getCompanyUser.ContactUsId)
            );

            if (contact == null)
            {
                TempData["ErrorMessage"] = "User not found. Please login or contact support.";
                return RedirectToAction("Index", "Home", new { area = "Company2" });
            }

            var allBookingCourse = unitOfWork.BookingCourses.Get(e =>
                (getCompanyUser != null && e.CompanyUserId == appUserId) ||
                (e.FirstPayment.ContactUsId == contact.Id)
            );

            if (allBookingCourse.Any(e => e.CourseId == id))
            {
                TempData["InfoMessage"] = "You Already Enrolled In This Course";
                return RedirectToAction("Index", "Home", new { area = "Company2" });
            }

            var exisitCousre = unitOfWork.Carts.GetOne(e =>
                (token != null && e.ContactUs.Token == token && e.CourseId == id) ||
                (getCompanyUser != null && e.ContactUsId == getCompanyUser.ContactUsId && e.CourseId == id),
                includes: [e => e.ContactUs]
            );

            if (exisitCousre == null)
            {
                var cart = new Cart
                {
                    ContactUsId = contact.Id,
                    CourseId = id
                };

                unitOfWork.Carts.Create(cart);
                unitOfWork.Commit();
            }
            else
            {
                TempData["InfoMessage"] = "This course is already in your cart.";
            }

            return RedirectToAction("Index", "Home", new { area = "Company2" });
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var course = unitOfWork.Courses.GetOne(c => c.Id == id, includes: [e => e.Chapters, e => e.Lessons]);
            if (course == null)
            {
                return RedirectToAction("Error", "Courses");
            }
            return View(course);
        }
    }
}
