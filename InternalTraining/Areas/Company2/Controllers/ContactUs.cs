using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternalTraining.Areas.Company2.Controllers
{
    [Area("Company2")]
    public class ContactUs : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public ContactUs(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(CompanyContactUs companyContactUs)
        {
            var currentUser = userManager.GetUserId(User);
            companyContactUs.ApplicationUserId = currentUser;
            companyContactUs.IsAnswered = false;
            if (ModelState.IsValid)
            {
                unitOfWork.CompnanyContactsUs.Create(companyContactUs); 
                unitOfWork.Commit();
                TempData["SuccessMessage"] = "Your Message sent Successfully. Please Wait for the Response and Check Your Email.";

                return RedirectToAction("Index", "Home", new { area = "Company2" });
            }

            return View();  
            
        }
    }
}
