using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models.ViewModel
{
    public class LoginVm
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string UserType { get; set; }

        public List<SelectListItem> UserTypes { get; set; } = new List<SelectListItem>
         {
             new SelectListItem { Text = "Admin", Value = "Admin" },
             new SelectListItem { Text = "Company", Value = "Company" },
             new SelectListItem { Text = "Employee", Value = "Employee" }
         };
    }
}
