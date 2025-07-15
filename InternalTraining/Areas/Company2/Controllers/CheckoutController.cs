using InternalTraining.Email_Sender;
using InternalTraining.Models.ViewModel;
using InternalTraining.Models;
using InternalTraining.Test_Cookie;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Stripe.Checkout;
using System.Linq;

namespace InternalTraining.Areas.Company2.Controllers
{
    [Area("Company2")]
    [CookieOrRoleAuthorize("ContactToken", "Company")]

    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;

        private readonly IUnitOfWork unitOfWork;
        public CheckoutController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
            
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Success(int firstPaymentId)
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);

            var appUserId = userManager.GetUserId(User);
            var getCompanyUser = unitOfWork.CompanyUsers.GetOne(e => e.Id == appUserId, includes: [e => e.ApplicationUser]);

            var firstPayment = unitOfWork.FirstPayments.GetOne(e =>
                (contact != null && e.Id == firstPaymentId && e.ContactUs.Token == contact.Token) ||
                (getCompanyUser != null && e.Id == firstPaymentId && getCompanyUser.ContactUsId == e.ContactUsId));

            if (firstPayment != null)
            {
                var service = new SessionService();
                var session = service.Get(firstPayment.SessionId);

                firstPayment.PaymentStripId = session.PaymentIntentId;
                firstPayment.Status = true;
                firstPayment.PaymentStatus = true;

                unitOfWork.Commit();

                // Empty the cart after successful payment
                var itemToDelete = unitOfWork.BookingCourses.Get(e => e.FirstPaymentId == firstPaymentId).ToList();
                foreach (var item in itemToDelete)
                {
                    var itemToDeleteInCart = unitOfWork.Carts.GetOne(e =>
                        e.CourseId == item.CourseId &&
                        ((contact != null && e.ContactUs.Token == contact.Token) || (getCompanyUser != null && getCompanyUser.ContactUsId == e.ContactUsId)));
                    if (itemToDeleteInCart != null)
                    {
                        unitOfWork.Carts.Delete(itemToDeleteInCart);
                    }
                }
                unitOfWork.Commit();
            }

            if (getCompanyUser == null)
            //if (getCompanyUser == null)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    FirstName = contact.CompanyName,
                    UserName = contact.Email,
                    Email = contact.Email
                };
                var password = "123456789@Company";
                var result = await userManager.CreateAsync(appUser, password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(appUser, false);
                    await userManager.AddToRoleAsync(appUser, "Company");

                    var companyUserinDb = new CompanyUser
                    {
                        Id = appUser.Id,
                        ContactUsId = contact.Id,
                        CompanyName = contact.CompanyName,
                    };

                    unitOfWork.CompanyUsers.Create(companyUserinDb);
                    unitOfWork.Commit();

                    var getbookingCourses = unitOfWork.BookingCourses.Get(e => e.FirstPaymentId == firstPaymentId);
                    foreach (var booking in getbookingCourses)
                    {
                        booking.CompanyUserId = appUser.Id;
                    }
                    unitOfWork.Commit();

                    var companyUser = await userManager.FindByEmailAsync(contact.Email);
                    var companytoken = await userManager.GeneratePasswordResetTokenAsync(companyUser);
                    string subject = "Welcome to Mentor You Are Now One of Our Members ";

                    // i need to adujst the direction of the link.. 
                    
                    var link = Url.Action("ResetPassword", "Account", new { area = "Identity", token = companytoken, email = contact.Email }, Request.Scheme);

                    string body = $@"
                             <div style='font-family: Arial, sans-serif;'>
                             <h1 style='color: #143D60; font-weight: bold;'>Mentor</h1>
                             <p>Hello {contact.CompanyName},</p>
                             <p>Thank you for contacting us.</p>
                             <p>Please <a href='{link}' style='color: #1a73e8;'>click here</a> to login to your dashboard.</p>
                             <br/>
                                 <p>Best regards,<br/>Mentor Team</p>
                                      </div>";
                    await emailSender.SendEmailAsync(contact.Email, subject, body);

                    TempData["SuccessMessage"] = "Please check your email.";

                    return RedirectToAction("Index", "Home", new { area = "Company2" });
                }
            }
            else
            {
                var currentUser = userManager.GetUserId(User);
                if (currentUser != null)
                {
                    var getbookingCourses = unitOfWork.BookingCourses.Get(e => e.FirstPaymentId == firstPaymentId && e.CompanyUserId == null);
                    foreach (var booking in getbookingCourses)
                    {
                        booking.CompanyUserId = currentUser;
                    }
                    unitOfWork.Commit();
                }
            }

            return RedirectToAction("Index", "Home", new { area = "AdminCompany" });
        }

      
        public IActionResult Cancel()
        {
            return View();
        }

        // to save the company in my asp.net users with role Company

        public IActionResult Login()
        {
            return View();
        }

    }
}
