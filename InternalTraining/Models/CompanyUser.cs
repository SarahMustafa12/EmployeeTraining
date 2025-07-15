using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalTraining.Models
{
    public class CompanyUser
    {

        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }  // Same as ApplicationUser.Id (string FK and PK)

        public string CompanyName { get; set; }

        public string? ProfilePicturePath { get; set; }
        public int ContactUsId { get; set; }
        public ContactUs ContactUs { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<EmployeeUser> EmployeeUsers { get; set; } 

        public ICollection<BookingCourse> BookingCourses { get; set; } = new List<BookingCourse>();

    }
}
