using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace InternalTraining.Areas.Company2.Controllers
{
    [Area("Company2")]
    public class CheckoutController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CheckoutController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Success(int firstPaymentId)
        {
            var token = Request.Cookies["ContactToken"];
            var contact = unitOfWork.ContactsUs.GetOne(e => e.Token == token);
            var firstPayment = unitOfWork.FirstPayments.GetOne(e=>e.Id == firstPaymentId && e.ContactUs.Token ==contact.Token);
            if (firstPayment != null)
            {
                var service = new SessionService();
                var session = service.Get(firstPayment.SessionId);

                firstPayment.PaymentStripId = session.PaymentIntentId;
                firstPayment.Status = true;
                firstPayment.PaymentStatus = true;

                unitOfWork.Commit();    
                //empty the cart after success pahyment

                var itemToDelete = unitOfWork.BookingCourses.Get(e=>e.FirstPaymentId == firstPaymentId).ToList();
                foreach (var item in itemToDelete)
                {
                    var itemToDeleteInCart = unitOfWork.Carts.GetOne(e=>e.CourseId == item.CourseId && e.ContactUs.Token == contact.Token);
                    unitOfWork.Carts.Delete(itemToDeleteInCart);
                    unitOfWork.Commit() ;
                }
            }
            return View();
        } 

        public IActionResult Cancel()
        {
            return View();
        }
    }
}
