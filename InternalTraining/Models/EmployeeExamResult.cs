using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InternalTraining.Models
{
    public class EmployeeExamResult
    {
        public int Id { get; set; }

        public string EmployeeUserId { get; set; }
        [ValidateNever]
        public EmployeeUser EmployeeUser { get; set; } = null!;

        public int ExamId { get; set; }
        [ValidateNever]
        public Exam Exam { get; set; } = null!;

        public double Score { get; set; }

        public DateTime TakenAt { get; set; } = DateTime.Now;

        public bool IsPassed { get; set; }
    }
}
