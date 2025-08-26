using Chatbot.Common.Extension;
using Chatbot.Common.Result;
using Chatbot.Service;
using Chatbot.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chatbot.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatbotService _chatbotService;

        public HomeController(ILogger<HomeController> logger, IChatbotService chatbotService)
        {
            _logger = logger;
            _chatbotService = chatbotService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Chatbot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Chatbot(string message)
        {
            try
            {
                var chatbot = await _chatbotService.Ask(message);
                if (string.IsNullOrWhiteSpace(message)) return Ok(new Result<bool> { IsSuccessed = false, Message = "Vui lòng nhập câu hỏi" });
                return Ok(new Result<bool> { IsSuccessed = true, Message = chatbot.Response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi {0}", Request.GetFullUrl());
                return StatusCode(500, "Đã có lỗi xảy ra");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
