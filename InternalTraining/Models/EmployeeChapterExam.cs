namespace InternalTraining.Models
{
    public class EmployeeChapterExam
    {
        public int Id { get; set; }
        public string EmployeeUserId { get; set; }
        public EmployeeUser EmployeeUser { get; set; } = null!;

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; } = null!;

        public double Score { get; set; }
        public DateTime TakenOn { get; set; }
    }
}
