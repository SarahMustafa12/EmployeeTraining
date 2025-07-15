using InternalTraining.Data.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The is required")]

        public string Text { get; set; }
        [Required]
        public QuestionType QuestionType { get; set; }

        public int ExamId { get; set; }
        [ValidateNever]
        public Exam Exam { get; set; } = null!;
        [ValidateNever]
        public List<Option> Options { get; set; } = new List<Option>();
        [Required]
        public int CorrectAnswer { get; set; }
    }
}
