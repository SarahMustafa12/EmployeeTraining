namespace InternalTraining.Models
{
    public class CourseFeedback
    {
        public int Id { get; set; }
        public string EmployeeUserId { get; set; }
        public EmployeeUser EmployeeUser { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public string? Comment { get; set; }
        public int Rating { get; set; }  

    }
}
