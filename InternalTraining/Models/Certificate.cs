namespace InternalTraining.Models
{
    public class Certificate
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; } = null!;


        public DateTime IssueDate { get; set; }

        public double Score { get; set; } 

        public string CertificateUrl { get; set; } 
    }
}
