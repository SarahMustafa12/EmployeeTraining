using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalTraining.Models
{
    public class Employee
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } //Foreign Key to Application user 
        public string ProfilePicturePath { get; set; }
        public string Department { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Certificate> Certificates { get; set; }

        public ICollection<EmployeeProgress> Progresses { get; set; } = new List<EmployeeProgress>();



    }
}
