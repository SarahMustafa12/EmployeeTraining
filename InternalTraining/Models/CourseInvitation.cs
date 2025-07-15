using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models
{
    public class CourseInvitation
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; } // invited employee's email (not yet a user)
        public int CourseId { get; set; }
        [ValidateNever]
        public string CompanyUserId { get; set; }  // invited employee's company user ID (may differ)

        public bool IsAccepted { get; set; } = false;
        public DateTime SentAt { get; set; }
        [ValidateNever]
        public Course Course { get; set; }
        [ValidateNever]
        public CompanyUser CompanyUser { get; set; }

        // FK to the bulk payment that covers this invitation (nullable if not paid yet)
        public int? SecondPaymentId { get; set; }
        [ValidateNever]
        public SecondPayment? SecondPayment { get; set; }

        public bool Paid { get; set; } = false;
        public string? Message { get; set; }

        //    public int Id { get; set; }
        //    public string Email { get; set; } // employee's email (not yet a user)
        //    public int CourseId { get; set; }
        //    public string CompanyUserId { get; set; }

        //    public bool IsAccepted { get; set; } = false;
        //    public DateTime SentAt { get; set; }

        //    public Course Course { get; set; }
        //    public CompanyUser CompanyUser { get; set; }
        //    //public int SecondPaymentId { get; set; }
        //    //public SecondPayment SecondPayment { get; set; }
        //    public bool Paid { get; set; } = false;
        //    public string? Message { get; set; }    


    }
}
