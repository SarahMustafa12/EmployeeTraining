using InternalTraining.Email_Sender;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternalTraining.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyContactController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;

        public CompanyContactController(IUnitOfWork unitOfWork,IEmailSender emailSender)
        {
            this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
        }
        public IActionResult Index(string? query, bool? notAnsw = false, bool? Answerd = false)
        {
        
            var allContacts = unitOfWork.CompnanyContactsUs.Get(includes: [e=>e.ApplicationUser]);

            if (notAnsw == true)
            {
                allContacts = unitOfWork.CompnanyContactsUs.Get(e=>e.IsAnswered == false,includes: [e => e.ApplicationUser]);
            }
            else if (Answerd== true)
            {
                allContacts = unitOfWork.CompnanyContactsUs.Get(e => e.IsAnswered == true, includes: [e => e.ApplicationUser]);
            }
            else
            {
                if(query != null)
                {
                    allContacts = unitOfWork.CompnanyContactsUs.Get(e => e.ApplicationUser.Email.Contains(query),includes: [e => e.ApplicationUser]);
                }
            }
            return View(allContacts.ToList());
        }
        [HttpGet]
        public IActionResult Contact(int id)
        {
            var contact = unitOfWork.CompnanyContactsUs.GetOne(e => e.Id == id, includes: [e => e.ApplicationUser]);

            return View(contact);
           
        }
        [HttpPost]
        public async Task<ActionResult> Contact(int id, string? ReplyMessage)
        {
            
            var contact = unitOfWork.CompnanyContactsUs.GetOne(e=>e.Id == id,includes: [e => e.ApplicationUser]);
            var email = contact.ApplicationUser.Email;

             if(contact != null && email!= null)
            {


                string subject = "Mentor Team";

                string body = $"<p>{ReplyMessage.Replace("\n", "<br/>")}</p>";

                await emailSender.SendEmailAsync(email, subject, body);

             
                contact.IsAnswered = true;
                unitOfWork.CompnanyContactsUs.Update(contact);
                unitOfWork.Commit();

                TempData["InfoMessage"] = $"Your Reply Was Sent Successfully for {contact.ApplicationUser.Email} ";
                return RedirectToAction("index", "CompanyContact", new { area = "Admin" });


            }


            return NotFound();
        }

        
    }
}
