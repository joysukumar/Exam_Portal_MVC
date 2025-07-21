using Exam_Portal.Interface;
using Exam_Portal.Models;
using Exam_Portal.Models.ViewModel;
using Exam_Portal_Web_Api.Models;
using Microsoft.EntityFrameworkCore;


namespace Exam_Portal.QuestionService
{
    public class QuestionService : IQuestionService
    {
        private readonly Appdbcontext _context;
        public QuestionService(Appdbcontext context) { _context = context; }

        public async Task<List<Question>> GetAllQuestionsAsync() => await _context.Questions.Include(q => q.Options).ToListAsync();

        public async Task<QuestionFormVM> GetQuestionFormVMAsync(int questionId)
        {
            var question = await _context.Questions.Include(q => q.Options)
                                                   .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null) return null;

            var correctIndex = question.Options.FindIndex(o => o.Id == question.CorrectOptionId);

            return new QuestionFormVM
            {
                Text = question.Text,
                OptionTexts = question.Options.Select(o => o.Text).ToList(),
                CorrectOptionIndex = correctIndex
            };
        }

        public async Task UpdateQuestionAsync(int questionId, QuestionFormVM vm)
        {
            var question = await _context.Questions.Include(q => q.Options)
                                                   .FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null) return;

            question.Text = vm.Text;

            for (int i = 0; i < question.Options.Count && i < vm.OptionTexts.Count; i++)
            {
                question.Options[i].Text = vm.OptionTexts[i];
                
            }

            question.CorrectOptionId = question.Options[vm.CorrectOptionIndex].Id;
           
                _context.Questions.Update(question);
                await _context.SaveChangesAsync();
         
        }

        public async Task AddQuestionAsync(QuestionFormVM vm)
        {
            var question = new Question
            {
                Text = vm.Text,
                Options = vm.OptionTexts.Select(text => new Option { Text = text }).ToList()
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            // Set the correct OptionId now that IDs are created
            question.CorrectOptionId = question.Options[vm.CorrectOptionIndex].Id;

            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var q = await _context.Questions.FindAsync(id);
            if (q != null)
            {
                _context.Questions.Remove(q);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveUserAnswerAsync(string userId, int questionId, int selectedOptionId)
        {
            var existing = await _context.UserAnswers.FirstOrDefaultAsync(u => u.UserId == userId && u.QuestionId == questionId);
            if (existing != null)
            {
                existing.SelectedOptionId = selectedOptionId;
            }
            else
            {
                _context.UserAnswers.Add(new UserAnswer { UserId = userId, QuestionId = questionId, SelectedOptionId = selectedOptionId });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<int> CalculateScoreAsync(string userId)
        {
            var answers = await _context.UserAnswers.Where(x => x.UserId == userId).ToListAsync();
            var score = 0;
            foreach (var ans in answers)
            {
                var question = await _context.Questions.FindAsync(ans.QuestionId);
                if (question != null && ans.SelectedOptionId == question.CorrectOptionId)
                    score++;
            }
            return score;
        }

        public async Task<List<UserAnswer>> GetUserAnswersAsync(string userId) => await _context.UserAnswers.Where(x => x.UserId == userId).ToListAsync();

        public async Task<bool> HasUserCompletedExamAsync(string userId)
        {
            return await _context.ExamStatuses.AnyAsync(e => e.UserId == userId && e.IsCompleted);
        }

        public async Task MarkExamAsCompletedAsync(string userId)
        {
            var existing = await _context.ExamStatuses.FirstOrDefaultAsync(e => e.UserId == userId);
            if (existing != null)
            {
                existing.IsCompleted = true;
                _context.ExamStatuses.Update(existing);
            }
            else
            {
                _context.ExamStatuses.Add(new ExamStatus { UserId = userId, IsCompleted = true });
            }
            await _context.SaveChangesAsync();
        }

    }

}
