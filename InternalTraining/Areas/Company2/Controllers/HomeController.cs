using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Mvc;

namespace InternalTraining.Areas.Company2.Controllers
{
    [Area("Company2")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index(string token)
        {
            if (token == null)
            {
                return RedirectToAction("AccessDenied","Account", new {area="Identity"} );
            }
            var company = unitOfWork.ContactsUs.GetOne(e => e.Token == token);
            if (company != null)
            {
                Response.Cookies.Append("ContactToken", company.Token);
            }
           
            var courses = unitOfWork.Courses.Get(includes: [e=>e.Chapters, e=>e.Lessons]);
            return View(courses.ToList());
        }
        public IActionResult Pricing()
        {

            return View();
        }


        public IActionResult Enroll(int id)
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);
            if (contact != null)
            {
                var exisitCousre = unitOfWork.Carts.GetOne(e=>e.ContactUs.Token == token && e.CourseId == id, includes: [e=>e.ContactUs]);
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

            }

            return RedirectToAction("Index", "Home", new {area = "Company2"});
        }
    }
}
