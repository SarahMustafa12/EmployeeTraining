using InternalTraining.Models;
using InternalTraining.Test_Cookie;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Linq;

namespace InternalTraining.Areas.Company2.Controllers
{
    [Area("Company2")]
    [CookieOrRoleAuthorize("ContactToken", "Company")]

    //[Authorize(Roles = "Company")]

    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        public CartController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        // this action display the cart item...
        public IActionResult Index()
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);

            var appUserId = userManager.GetUserId(User);
            var getCompanyUser = unitOfWork.CompanyUsers.GetOne(e => e.Id == appUserId);

            var cartItems = unitOfWork.Carts.Get(e=>( contact != null && e.ContactUs.Token == contact.Token) ||(getCompanyUser!=null && e.ContactUs.Id == getCompanyUser.ContactUsId ), includes: [e=>e.ContactUs, e=>e.Course]);
            var totalPrie = cartItems.Sum(e=>e.Course.Price);
            ViewBag.TotalPrie = totalPrie;

            return View(cartItems.ToList());
        }
        // to display the cart count

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);

            var appUserId = userManager.GetUserId(User);
            var getCompanyUser = unitOfWork.CompanyUsers.GetOne(e => e.Id == appUserId);

            int cartCount = 0;

            if (contact != null || getCompanyUser != null)
            {
                cartCount = unitOfWork.Carts.Get(e =>(contact != null && e.ContactUsId == contact.Id) ||
                    (getCompanyUser != null && e.ContactUsId == getCompanyUser.ContactUsId)).Count();
            }

            return Json(cartCount);
        }

       
        public IActionResult Delete(int courseId)
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);

            var appUserId = userManager.GetUserId(User);
            var getCompanyUser = unitOfWork.CompanyUsers.GetOne(e => e.Id == appUserId);



            var theDeletedCourse = unitOfWork.Carts.GetOne(e => ( contact!= null && e.ContactUs.Token == contact.Token && e.CourseId == courseId) || (getCompanyUser!= null && e.ContactUsId ==getCompanyUser.ContactUsId && e.CourseId ==courseId), includes: [e => e.ContactUs, e => e.Course]);
            if (theDeletedCourse != null)
            {
                unitOfWork.Carts.Delete(theDeletedCourse);
                unitOfWork.Carts.Commit();  
            }
            return RedirectToAction("Index","Cart", new {area ="Company2"});
        }

        // the start of payment process 
        public IActionResult CheckOut()
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);

            var appUserId = userManager.GetUserId(User);
            var getCompanyUser = unitOfWork.CompanyUsers.GetOne(e => e.Id == appUserId);

            var cartItems = unitOfWork.Carts.Get(e=> ( contact != null && e.ContactUs.Token == contact.Token) || (getCompanyUser != null && e.ContactUsId == getCompanyUser.ContactUsId), includes: [e => e.ContactUs, e => e.Course]);
            if (cartItems != null)
            {
                var firstPayment = new FirstPayment
                {
                    ContactUsId = (int)(contact != null ? contact.Id : getCompanyUser?.ContactUsId),

                    //ContactUsId = contact.Id,
                    BookingTime = DateTime.Now,
                    TotalPrice = (long)cartItems.Sum(e => e.Course.Price)
                };
                unitOfWork.FirstPayments.Create(firstPayment);
                unitOfWork.Commit();
                //"ResetPassword", "Account", new { area = "Identity", token = token }
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/Company2/Checkout/Success?firstPaymentId={firstPayment.Id}",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/Company2/Checkout/Cancel",
                };
                foreach (var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Course.Name,
                                Images = new List<string>
                            {
                                 $"{Request.Scheme}://{Request.Host}/images/admin/{item.Course.CourseImage}"
                            }

                            },
                            UnitAmount = (long)item.Course.Price*100,
                        },
                        Quantity = 1
                    });

                }

                var service = new SessionService();
                var session = service.Create(options);
                firstPayment.SessionId = session.Id;
                unitOfWork.Commit();

                List<BookingCourse> bookingCourses = [];
                foreach (var item in cartItems)
                {
                    var bookingCourse = new BookingCourse
                    {
                        FirstPaymentId = firstPayment.Id,
                        CourseId = item.CourseId,
                        Price = (long)item.Course.Price,
                        
                    };

                    bookingCourses.Add(bookingCourse);
                }

             unitOfWork.BookingCourses.Create(bookingCourses);
                unitOfWork.Commit();

                return Redirect(session.Url);
            }

            return View();
        }
    }
}
