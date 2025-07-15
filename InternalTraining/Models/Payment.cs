namespace InternalTraining.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public CompanyUser CompanyUser { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public bool PaymentStatus { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentStripId { get; set; }

    }
}
