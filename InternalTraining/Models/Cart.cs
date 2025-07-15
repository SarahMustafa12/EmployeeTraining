namespace InternalTraining.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int ContactUsId { get; set; }  
        public ContactUs ContactUs { get; set; }
    }
}
