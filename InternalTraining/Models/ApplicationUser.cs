using Microsoft.AspNetCore.Identity;
namespace InternalTraining.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Common properties
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePicturePath { get; set; }  // Path to profile picture

        // Relationships
        public CompanyUser? CompanyUser { get; set; }

        public EmployeeUser? EmployeeUser { get; set; }

        public ICollection<CompanyContactUs> CompanyContactsUs { get; set; }


    }

}
