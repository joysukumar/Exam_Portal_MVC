using System.ComponentModel.DataAnnotations;

namespace Exam_Portal.Models.ViewModel
{
    public class QuestionFormVM
    {
        [Required(ErrorMessage ="Required")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Required")]
        public List<string> OptionTexts { get; set; } = new() { "", "", "", "" };
        [Required(ErrorMessage = "Required")]
        public int CorrectOptionIndex { get; set; } // 0 to 3
    }

}
