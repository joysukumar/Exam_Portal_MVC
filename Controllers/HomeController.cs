using System.Diagnostics;
using Exam_Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Portal.Controllers
{
    public class HomeController : Controller
    {
    
        public IActionResult Index()
        {
            return View();
        }

     
    }
}
