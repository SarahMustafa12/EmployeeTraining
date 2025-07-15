using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models
{
    public class CompanyContactUs
    {
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [ValidateNever]
        public string? ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public bool? IsAnswered { get; set; }
    }
}
