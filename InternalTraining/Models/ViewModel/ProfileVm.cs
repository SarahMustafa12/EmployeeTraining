using System.ComponentModel.DataAnnotations;

namespace InternalTraining.Models.ViewModel
{
    public class ProfileVm
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string? ProfilePicture { get; set; }
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [DataType(DataType.Password)]

        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string? ConfirmPassword { get; set; }

    }
}
