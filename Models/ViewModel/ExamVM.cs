using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Exam_Portal.Models.ViewModel
{
    public class ExamVM
    {
        public int QuestionId { get; set; }
        [DisplayName("Question")]
        public string QuestionText { get; set; }
        public List<OptionVM> Options { get; set; }
       
        public int? SelectedOptionId { get; set; }
        public int CurrentQuestionNumber { get; set; }
        public int TotalQuestions { get; set; }
      
    }
}
