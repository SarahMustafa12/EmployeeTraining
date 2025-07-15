namespace InternalTraining.Models
{
    public class EmployeeCourse
    {

        
        public string EmployeeUserId { get; set; }
        public EmployeeUser EmployeeUser { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime InvitedOn { get; set; } = DateTime.Now;
        public bool IsAccepted { get; set; }
    }
}
