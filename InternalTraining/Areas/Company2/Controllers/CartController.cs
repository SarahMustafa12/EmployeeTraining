using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Linq;

namespace InternalTraining.Areas.Company2.Controllers
{
    [Area("Company2")]

    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // this action display the cart item...
        public IActionResult Index()
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);

            var cartItems = unitOfWork.Carts.Get(e=>e.ContactUs.Token == contact.Token, includes: [e=>e.ContactUs, e=>e.Course]);
            var totalPrie = cartItems.Sum(e=>e.Course.Price);
            ViewBag.TotalPrie = totalPrie;

            return View(cartItems.ToList());
        }
        // to display the cart count
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token).Token;
            int cartCount = unitOfWork.Carts.Get(e => e.ContactUs.Token == contact).ToList().Count;
            return Json(cartCount);
        }
        public IActionResult Delete(int courseId)
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);
            var theDeletedCourse = unitOfWork.Carts.GetOne(e => e.ContactUs.Token == contact.Token && e.CourseId ==courseId, includes: [e => e.ContactUs, e => e.Course]);
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
            var cartItems = unitOfWork.Carts.Get(e => e.ContactUs.Token == contact.Token, includes: [e => e.ContactUs, e => e.Course]);
            if (cartItems != null)
            {
                var firstPayment = new FirstPayment
                {
                    ContactUsId = contact.Id,
                    BookingTime = DateTime.Now,
                    TotalPrice = (long)cartItems.Sum(e => e.Course.Price)
                };
                unitOfWork.FirstPayments.Create(firstPayment);
                unitOfWork.Commit();

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
                        Price = (long)item.Course.Price
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
