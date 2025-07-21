using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Exam_Portal.Models.ViewModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name is required")]
      
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords donot match")]
        public string? ConfirmPassword { get; set; }
        
    }
}
