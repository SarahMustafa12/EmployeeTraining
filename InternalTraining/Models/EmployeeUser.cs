using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalTraining.Models
{
    public class EmployeeUser
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } 

        public string ProfilePicturePath { get; set; }
        public string Department { get; set; }

        public string CompanyUserId { get; set; }
        public CompanyUser CompanyUser { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        public ICollection<EmployeeProgress> Progresses { get; set; } = new List<EmployeeProgress>();

        public ICollection<EmployeeCourse> EmployeeCourses { get; set; }
    }
}
