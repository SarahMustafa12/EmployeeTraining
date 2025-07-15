using InternalTraining.Email_Sender;
using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
=======
>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace InternalTraining.Areas.AdminCompany.Controllers
{
    [Area("AdminCompany")]
<<<<<<< HEAD
    [Authorize(Roles = "Company")]
=======
>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;


        public EmployeeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var allEmp = unitOfWork.EmployeeCourses.Get(includes: [e=>e.EmployeeUser.ApplicationUser, e=>e.Course]);
            return View(allEmp.ToList());
        }

        [HttpGet]
        public IActionResult SendInvitaion(int id)
        {
            var currentUser = userManager.GetUserId(User);
            var enrolledCourses = unitOfWork.BookingCourses.GetOne(e => e.CompanyUserId == currentUser && e.CourseId == id, includes: [e => e.Course]);
            ViewBag.Course = enrolledCourses;

            return View();
        }

        [HttpPost]
        public IActionResult SendInvitaion(int courseId, List<CourseInvitation> courseInvitations)
        {
            var currentUser = userManager.GetUserId(User);

            var enrolledCourses = unitOfWork.BookingCourses.GetOne(e => e.CompanyUserId == currentUser && e.CourseId == courseId, includes: [e => e.Course]);

            ViewBag.Course = enrolledCourses;

            var allInvations = unitOfWork.CourseInvitations.Get(e=>e.CourseId == courseId && e.CompanyUserId == currentUser && e.Paid == true);

            if (allInvations.Count() != 0)
            {
                var invitedEmails = allInvations.Select(e => e.Email).ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var invitation in courseInvitations)
                {
                    if (invitedEmails.Contains(invitation.Email))
                    {
                        ModelState.AddModelError(string.Empty, $"Employee with email {invitation.Email} is already invited.");
                        //ModelState.AddModelError("Email", $"Employee with email {invitation.Email} is already invited.");
                    }
                }

                // Stop if there are any errors
                if (!ModelState.IsValid)
                {
                    // Optionally pass courseInvitations back to the view
                    return View(courseInvitations);
                }
            }


            var secondPayment = new SecondPayment
                {
                    CompanyUserId = currentUser,
                    PaymentDate = DateTime.Now,
                    CourseId = courseId,
                    Amount = courseInvitations.Count * 4000
                };

                unitOfWork.SecondPayments.Create(secondPayment);
                unitOfWork.Commit();
                foreach (var item in courseInvitations)
                {
                    item.CompanyUserId = currentUser;
                    item.SentAt = DateTime.Now;
                    item.SecondPaymentId = secondPayment.Id;
                }

                unitOfWork.CourseInvitations.Create(courseInvitations);
                unitOfWork.Commit();



                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/AdminCompany/Employee/Success?paymentId={secondPayment.Id}",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/AdminCompany/Employee/Cancel"
                };

                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Course Payment for {courseInvitations.Count} Employees",
                            Description = $"Pay For Employee Invitaions ",

                        },
                        UnitAmount = 4000 * 100,
                    },
                    Quantity = courseInvitations.Count

                });

                var service = new SessionService();
                var session = service.Create(options);
                secondPayment.SesstionId = session.Id;
                unitOfWork.Commit();



                return Redirect(session.Url);
            }
           
        

      
        public async Task<IActionResult> Success(int paymentId)
        {
            var currentUser = userManager.GetUserId(User);
            var secondPayment = unitOfWork.SecondPayments.GetOne(e => e.Id == paymentId && e.CompanyUserId == currentUser);
            var allSucessInvitaion = unitOfWork.CourseInvitations.Get(e => e.SecondPaymentId == paymentId && e.CompanyUserId == currentUser);

            if (secondPayment != null)
            {
                var service = new SessionService();
                var session = service.Get(secondPayment.SesstionId);
               secondPayment.StripePaymentId = session.PaymentIntentId;
               secondPayment.NumberOfEmployees = allSucessInvitaion.Count();
                unitOfWork.SecondPayments.Update(secondPayment);
                unitOfWork.Commit();

                foreach (var item in allSucessInvitaion)
                {
                    item.SecondPaymentId = paymentId;
                    item.Paid = true;
                    unitOfWork.Commit();

                    var appUser = new ApplicationUser
                    {
                        Email = item.Email,
                        UserName = item.Email,
                    };
                    var password = "123456789@Emp";
                    var result = await userManager.CreateAsync(appUser, password);
                    if (result.Succeeded)
                    {
                        
                        await userManager.AddToRoleAsync(appUser, "Employee");

                        var empUserinDb = new EmployeeUser
                        {
                            Id = appUser.Id,
                            CompanyUserId = currentUser,
                            Department = "anything",
                            ProfilePicturePath = "test.Png",

                        };

                        unitOfWork.EmployeeUsers.Create(empUserinDb);
                        unitOfWork.Commit();

                        // send an emial for each employee in to Reset Their Passwords.

                        var empUser = await userManager.FindByEmailAsync(item.Email);
                        var empToken = await userManager.GeneratePasswordResetTokenAsync(empUser);

                        string subject = "Welcome to Mentor You Are Now One of Our Members ";

                        // i need to adujst the direction of the link.. 

                        var link = Url.Action("ResetPassword", "Account", new { area = "Identity", token = empToken, email = item.Email }, Request.Scheme);


                        string body = $@"
                              <div style='font-family: Arial, sans-serif;'>
                              <h1 style='color: #143D60; font-weight: bold;'>Mentor</h1>
                              <p>Hello,</p>
                              <p>You have been invited by your company to join a training course on Mentor.</p>
                        <p>To begin, please <a href='{link}' style='color: #1a73e8;'>click here</a> to reset your password.</p>
                           <p>After setting your new password, you can log in using your email and the password you just created.</p>
                          <p>This course is designed to help you grow and develop your professional skills.</p>
                            <br/>
                           <p>We're excited to have you with us!</p>
                           <p>Best regards,<br/>Mentor Team</p>
                            </div>";

                        await emailSender.SendEmailAsync(item.Email, subject, body);
                    }

                    var empCourse = new EmployeeCourse
                    {
                        EmployeeUserId = appUser.Id,
                        CourseId = item.CourseId,
                        InvitedOn = DateTime.Now,   
                        IsAccepted = false,
                    };

                    unitOfWork.EmployeeCourses.Create(empCourse);   
                    unitOfWork.Commit();

                }

            }
            TempData["SuccessMessage"] = "All The Invitations are Sent Successfully.";

            return RedirectToAction("Index", "Home", new { area = "Company2" });

        }
    }
}
