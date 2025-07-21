using Exam_Portal.Models;
using Exam_Portal.Models.ViewModel;
using Exam_Portal_Web_Api.Models;

namespace Exam_Portal.Interface
{
    public interface IQuestionService
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<QuestionFormVM> GetQuestionFormVMAsync(int questionId);
        Task UpdateQuestionAsync(int questionId, QuestionFormVM vm);
        Task AddQuestionAsync(QuestionFormVM vm);
        Task<Question> GetQuestionByIdAsync(int id);

        Task DeleteQuestionAsync(int id);
        Task SaveUserAnswerAsync(string userId, int questionId, int selectedOptionId);
        Task<int> CalculateScoreAsync(string userId);
        Task<List<UserAnswer>> GetUserAnswersAsync(string userId);
        Task<bool> HasUserCompletedExamAsync(string userId);
        Task MarkExamAsCompletedAsync(string userId);

    }
}
