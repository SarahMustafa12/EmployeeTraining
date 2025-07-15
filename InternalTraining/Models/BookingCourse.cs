namespace InternalTraining.Models
{
    public class BookingCourse
    {
        public int Id { get; set; }
        public int FirstPaymentId { get; set; }
        public FirstPayment FirstPayment { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public double Price { get; set; }
        public string? CompanyUserId { get; set; }  
        public CompanyUser? CompanyUser { get; set; }

    }
}
