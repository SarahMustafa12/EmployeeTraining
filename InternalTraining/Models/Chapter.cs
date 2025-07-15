using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CourseId { get; set; }

        [ValidateNever]
        public Course Course { get; set; }


        public int NumberOfLessons { get; set; }

        [ValidateNever]
        public ICollection<Lesson> Lessons { get; set; }

        [ValidateNever]
        public Exam Exam { get; set; } = null!;

    }
}
