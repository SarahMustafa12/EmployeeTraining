using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternalTraining.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactUsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ContactUsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            var allMessage = unitOfWork.ContactsUs.Get().ToList();

            return View(allMessage);
        }

        public IActionResult ShowMess(int id)
        {
            var message = unitOfWork.ContactsUs.GetOne(e => e.Id == id);

            return View(message);
        }

        public IActionResult SendEmail(int id)
        {
            var message = unitOfWork.ContactsUs.GetOne(e => e.Id == id);

            return View();
        }


        public IActionResult Delete(int id)
        { 

            return View();
        }
    }
}
