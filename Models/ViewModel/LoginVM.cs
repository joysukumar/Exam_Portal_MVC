using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Exam_Portal.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Username is required")]
     
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
       
    }
}
