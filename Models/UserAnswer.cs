namespace Exam_Portal.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
    }
}
