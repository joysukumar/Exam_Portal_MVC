using Exam_Portal.Interface;
using Exam_Portal.Models;
using Exam_Portal.Models.ViewModel;
using Exam_Portal.QuestionService;
using Exam_Portal_Web_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam_Portal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IQuestionService _service;
        private readonly UserManager<User> _userManager;

        public AdminController(IQuestionService service, UserManager<User> userManager)
        { _service = service;
            _userManager = userManager;
                }
        public async Task<IActionResult> Dashboard() => View(await _service.GetAllQuestionsAsync());

        public IActionResult Create()
        {
            return View(new QuestionFormVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionFormVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            for (int i = 0; i < model.OptionTexts.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(model.OptionTexts[i]))
                {
                    ModelState.AddModelError($"OptionTexts[{i}]", $"Option {i + 1} is required.");
                }
            }
            if (!ModelState.IsValid)
                return View(model);

            await _service.AddQuestionAsync(model);
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetQuestionFormVMAsync(id);
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, QuestionFormVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            for (int i = 0; i < model.OptionTexts.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(model.OptionTexts[i]))
                {
                    ModelState.AddModelError($"OptionTexts[{i}]", $"Option {i + 1} is required.");
                }
            }
            if (!ModelState.IsValid)
                return View(model);
            await _service.UpdateQuestionAsync(id, model);
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteQuestionAsync(id);
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> UserScores()
        {
            var users = _userManager.Users.ToList();
            var scores = new List<UserScoreVM>();

            foreach (var user in users)
            {
                // Exclude Admins
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    var score = await _service.CalculateScoreAsync(user.Id);
                    scores.Add(new UserScoreVM
                    {
                        Name = user.Name,  
                        Score = score
                    });
                }
            }

            return View(scores);
        }

    }

}
