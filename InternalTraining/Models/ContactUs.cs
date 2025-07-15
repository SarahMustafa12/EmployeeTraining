using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InternalTraining.Models
{
    public class ContactUs
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }  
        [Required]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Enter a valid phone number.")]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int NumberOfEmployees { get; set; }

        public bool? IsSeen { get; set; }

        public  bool? IsAnswered { get; set; }

        public DateTime ExpectedTrainingDate { get; set; }
        [ValidateNever]
        public string Message { get; set; }

        public string? Token { get; set; }



    }
}
