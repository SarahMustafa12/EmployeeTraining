namespace InternalTraining.Models
{
    public class FirstPayment
    {
        public int Id { get; set; }
        public int ContactUsId { get; set; }
        public ContactUs ContactUs { get; set; }
        public double TotalPrice { get; set; }
        public DateTime BookingTime { get; set; }

        public bool Status { get; set; }
        public bool PaymentStatus { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentStripId { get; set; }
    }
}
