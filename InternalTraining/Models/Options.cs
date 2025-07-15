using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models
{
    public class Option
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public int QuestionId { get; set; }
        [ValidateNever]
        public Question Question { get; set; }
    }
}
