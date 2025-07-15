using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models.ViewModel
{
    public class ForgetPasswordVm
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
