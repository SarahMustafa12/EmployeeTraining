using InternalTraining.Email_Sender;
using InternalTraining.Models;
using InternalTraining.Test_Cookie;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternalTraining.Areas.Company.Controllers
{
    [Area("Company")]
   
    public class ContactUsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;
  

        public ContactUsController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
;
        }
        [HttpGet]
        public IActionResult Index()
        {


            return View();
        }
        // filling the form
        [HttpPost]
        public async Task<IActionResult> Index(ContactUs contact)
        {
                if (ModelState.IsValid)
                {
                    if (contact != null)
                    {
                        contact.Token = Guid.NewGuid().ToString();
                        unitOfWork.ContactsUs.Create(contact);
                        unitOfWork.Commit();
                    }

                    // send email to the company with a link of a page that it could enroll in a courses.

                    string subject = "Welcome to Mentor — Enroll in Courses";

                    // i need to adujst the direction of the link.. 

                    var link = Url.Action("AccessViaToken", "Home", new { area = "Company2", token = contact.Token }, Request.Scheme);

                    string body = $@"
                             <div style='font-family: Arial, sans-serif;'>
                             <h1 style='color: #143D60; font-weight: bold;'>Mentor</h1>
                             <p>Hello {contact.CompanyName},</p>
                             <p>Thank you for contacting us.</p>
                             <p>Please <a href='{link}' style='color: #1a73e8;'>click here</a> to enroll in our courses.</p>
                             <br/>
                                 <p>Best regards,<br/>Mentor Team</p>
                                      </div>";
                    await emailSender.SendEmailAsync(contact.Email, subject, body);

                    TempData["SuccessMessage"] = "Thank you for filling out the form. Please check your email.";

                    return RedirectToAction("Index", "Home", new { area = "Company" });
                }
                else

                {
                    return View(contact);
                }
           
        }
    }
}
