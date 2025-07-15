using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalTraining.Models
{
    public class Company
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }  // ForeignKey to ApplicationUser
        public string CompanyName { get; set; }

        public string ProfilePicturePath { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Employee> Employees { get; set; } 

        public ICollection<CompanyCourse> CompanyCourses { get; set; } = new List<CompanyCourse>();

    }
}
