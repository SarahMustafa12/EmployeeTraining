namespace InternalTraining.Models
{
    public class EmployeeProgress
    {
        public int Id { get; set; }
        public string EmployeeUserId { get; set; }
        public EmployeeUser EmployeeUser { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int LessonsCompleted { get; set; }
        public double ExamScore { get; set; }

        
    }
}
