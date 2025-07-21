using Exam_Portal.Interface;
using Exam_Portal.Models;
using Exam_Portal.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Register_Login_Test.Controllers
{
    [Authorize(Roles = "User")]
    public class ExamController : Controller
    {
        private readonly IQuestionService _service;
        private readonly UserManager<User> _userManager;

        public ExamController(IQuestionService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public async Task<IActionResult> StartExam()
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _service.HasUserCompletedExamAsync(user.Id))
            {
                var score = await _service.CalculateScoreAsync(user.Id);
                var totalQuestions = (await _service.GetAllQuestionsAsync()).Count;

                return View("ExamCompleted", new ExamCompletedVM
                {
                    Score = score,
                    TotalQuestions = totalQuestions
                });
            }

            var questions = await _service.GetAllQuestionsAsync();
            return RedirectToAction("Question", new { id = questions.First().Id });
        }



        public async Task<IActionResult> Question(int id)
        {
            var question = await _service.GetQuestionByIdAsync(id);
            var allQuestions = await _service.GetAllQuestionsAsync();
            var vm = new ExamVM
            {
                QuestionId = question.Id,
                QuestionText = question.Text,
                Options = question.Options.Select(o => new OptionVM { Id = o.Id, Text = o.Text }).ToList(),
                CurrentQuestionNumber = allQuestions.IndexOf(question) + 1,
                TotalQuestions = allQuestions.Count
            };
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Question(ExamVM model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (model.SelectedOptionId.HasValue)
            {
                await _service.SaveUserAnswerAsync(user.Id, model.QuestionId, model.SelectedOptionId.Value);
            }

            var questions = (await _service.GetAllQuestionsAsync()).OrderBy(q => q.Id).ToList();
            var currentIndex = questions.FindIndex(q => q.Id == model.QuestionId);

            if (currentIndex + 1 < questions.Count)
                return RedirectToAction("Question", new { id = questions[currentIndex + 1].Id });
            else
                return RedirectToAction("Result");
        }


        public async Task<IActionResult> Result()
        {
            var allQuestions = await _service.GetAllQuestionsAsync();
            var user = await _userManager.GetUserAsync(User);
            var score = await _service.CalculateScoreAsync(user.Id);
            ViewBag.Score = score;
            ViewBag.TotalQuestions = allQuestions.Count;

            await _service.MarkExamAsCompletedAsync(user.Id); // Mark as completed here

            return View();
        }


        public async Task<IActionResult> ViewAnswers()
        {
            var user = await _userManager.GetUserAsync(User);
            var answers = await _service.GetUserAnswersAsync(user.Id);
            var questions = await _service.GetAllQuestionsAsync();
            return View((answers, questions));
        }

    

    }
}
