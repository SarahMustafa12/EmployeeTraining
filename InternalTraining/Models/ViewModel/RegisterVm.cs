using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models.ViewModel
{
    public class RegisterVm
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
