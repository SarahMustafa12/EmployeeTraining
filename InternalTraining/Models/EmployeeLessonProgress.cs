namespace InternalTraining.Models
{
    public class EmployeeLessonProgress
    {
        public int Id { get; set; }
        public string EmployeeUserId { get; set; }
        public EmployeeUser EmployeeUser { get; set; } = null!;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public bool IsCompleted { get; set; }
        public DateTime CompletedOn { get; set; }
    }
}
