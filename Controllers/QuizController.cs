using Microsoft.AspNetCore.Mvc;
using QuizBHXH.Models;
using System.Text.Json;

namespace QuizBHXH.Controllers
{
    public class QuizController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public QuizController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // =========================
        // ĐỌC FILE JSON
        // =========================
        private List<Question> GetQuestions()
        {
            string filePath = Path.Combine(
                _env.ContentRootPath,
                "Data",
                "questions.json"
            );

            string json = System.IO.File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<Question>>(json);
        }

        // =========================
        // HIỂN THỊ CÂU HỎI
        // =========================
        public IActionResult Index(int id = 1)
        {
            var questions = GetQuestions();

            // Hết câu hỏi
            if (id > questions.Count)
            {
                ViewBag.Score = TempData["Score"];
                ViewBag.Total = questions.Count;

                return View("Finish");
            }

            var question = questions
                .FirstOrDefault(q => q.Id == id);

            ViewBag.Current = id;
            ViewBag.Total = questions.Count;

            return View(question);
        }

        // =========================
        // KIỂM TRA ĐÁP ÁN
        // =========================
        [HttpPost]
        public IActionResult CheckAnswer(
            int questionId,
            string answer
        )
        {
            var questions = GetQuestions();

            var q = questions
                .FirstOrDefault(x => x.Id == questionId);

            int score = 0;

            // Lấy điểm cũ
            if (TempData["Score"] != null)
            {
                score = Convert.ToInt32(
                    TempData["Score"]
                );
            }

            // KIỂM TRA ĐÚNG / SAI
            if (answer == q.CorrectAnswer)
            {
                TempData["Message"] =
                    "Minh thế mà lại hay 😎";

                score++;
            }
            else
            {
                TempData["Message"] =
                    "Ngu như bò 😭";

                // HIỆN ĐÁP ÁN ĐÚNG
                TempData["CorrectAnswer"] =
                    $"Đáp án đúng là: {q.CorrectAnswer}";
            }

            // Lưu điểm
            TempData["Score"] = score;

            // Sang trang kết quả
            return RedirectToAction(
                "Result",
                new { id = questionId + 1 }
            );
        }

        // =========================
        // TRANG KẾT QUẢ MỖI CÂU
        // =========================
        public IActionResult Result(int id)
        {
            ViewBag.Message =
                TempData["Message"];

            ViewBag.CorrectAnswer =
                TempData["CorrectAnswer"];

            ViewBag.NextId = id;

            return View();
        }
    }
}