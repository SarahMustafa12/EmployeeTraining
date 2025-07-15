namespace InternalTraining.Models
{
    public class EmployeeProgress
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int LessonsCompleted { get; set; }
        public double ExamScore { get; set; }

    }
}
