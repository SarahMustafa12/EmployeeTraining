namespace InternalTraining.Models
{
    public class SecondPayment
    {

        public int Id { get; set; }

        public string CompanyUserId { get; set; }  // The company user who made the payment (payer)

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public string? StripePaymentId { get; set; }
        public string? SesstionId { get; set; }
        public int NumberOfEmployees { get; set; }  // How many employees covered in this payment

        public int CourseId { get; set; }  // The course this payment is for

        public Course Course { get; set; }
        public CompanyUser CompanyUser { get; set; }

        // Navigation collection of all invitations this payment covers
        public ICollection<CourseInvitation> CourseInvitations { get; set; } = new List<CourseInvitation>();
    }
}
        //public int Id { get; set; }

        //public string CompanyUserId { get; set; }  // FK to the company user who paid

        //public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        //public decimal Amount { get; set; }        

        //public string? StripePaymentId { get; set; } 
        //public string? SesstionId { get; set; }
        //public int NumberOfEmployees { get; set; }  // How many employees covered in this payment

        //public int CourseId { get; set; }            // Which course this payment is for

        //public Course Course { get; set; }

        //public CompanyUser CompanyUser { get; set; }
   

