using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The is required")]
        [MinLength(3, ErrorMessage = "it must be more than 3 letters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The is required")]

        public int? ChapterId { get; set; }

        [ValidateNever]
        public Chapter Chapter { get; set; } = null!;
        [ValidateNever]
        public ICollection<Question> Questions { get; set; } = new List<Question>();

    }
}
