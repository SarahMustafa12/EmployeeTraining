namespace InternalTraining.Models
{
    public class CourseFeedback
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public string? Comment { get; set; }
        public int Rating { get; set; }  

    }
}
