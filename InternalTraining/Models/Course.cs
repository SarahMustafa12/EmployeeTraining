using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }


        [Required]
        [Range(3, 40, ErrorMessage = "Number of chapters must be between 3 and 40.")]
        public int NumberOfChapters { get; set; }

        
        [RegularExpression("^.*\\.(png|jpg)$")]
        public string? CourseImage { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 300 characters.")]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [ValidateNever]
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

        [ValidateNever]
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();


        [ValidateNever]
        public ICollection<CourseFeedback> Feedbacks { get; set; } = new List<CourseFeedback>();

        [ValidateNever]
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        [ValidateNever]
        public ICollection<BookingCourse> CompanyCourses { get; set; } = new List<BookingCourse>();

        [ValidateNever]
        public ICollection<EmployeeProgress> Progresses { get; set; } = new List<EmployeeProgress>();
        [ValidateNever]
        public ICollection<EmployeeCourse> EmployeeCourses { get; set; }
    }

}
