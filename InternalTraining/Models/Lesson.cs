using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InternalTraining.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [ValidateNever]
        public Chapter Chapter { get; set; }
        public int CourseId { get; set; }

        [ValidateNever]
        public Course Course { get; set; }
        public int ChapterId { get; set; }

        public string Title { get; set; }
        public string ContentUrl { get; set; }

    }
}
