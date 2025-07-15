namespace InternalTraining.Models
{
    public class CompanyCourse
    {
        public int Id { get; set; }

        public string CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;

        public string? PaymentStatus { get; set; }

    }
}
