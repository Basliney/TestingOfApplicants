using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingOfApplicants.Models;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Controllers
{
    /// <summary>
    /// Контроллёр теста
    /// </summary>
    public class TestController : Controller
    {
        ApplicationContext _context;    // Получение БД
        private User _user = null;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public TestController(ApplicationContext context)
        {
            this._context = context;
        }

        private User GetUser()
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(x => x.Id.Equals(int.Parse(HttpContext.User.Identity.Name)));
            }
            catch
            {
                return null;
            }
            ViewBag.ActiveUser = user;
            return user;
        }

        /// <summary>
        /// Обработка Get запроса
        /// </summary>
        /// <param name="id">Номер теста</param>
        /// <returns>Вид</returns>
        [HttpGet]
        public async Task<ActionResult> Index(int id)
        {
            _user = GetUser();

            if (_user == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (await _context.TestHeaders.FirstOrDefaultAsync(x=>x.Id == id) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Если Пользователь еще не прошел этот тест
            if (await _context.CompletedTestsDto.FirstOrDefaultAsync(x => (x.TestId == id && x.UserId == _context.Users.FirstOrDefault(x => x.Email.Equals(HttpContext.User.Identity.Name)).Id)) == null)
            {
                List<Question> questionList = new List<Question>(); // Объявляем список вопросов
                questionList.AddRange(_context.Questions.Where(x => x.HeaderId == id)); // Заносим в список вопросы этого теста
                
                // Если вопросов минимум 5
                if (questionList.Count >= 5)
                {
                    ViewBag.ChoosedHeader = id;  //Запоминаем id теста
                    ViewBag.header = _context.TestHeaders.Where(x => x.Id == id).FirstOrDefault();   // Запоминаем шапку теста
                    return View(questionList);  // Вызываем вьюшку
                }
            }
            // Если пользователь уже прошел тест, то возвращаем на домашнюю страницу
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Обработка Post запроса
        /// </summary>
        /// <param name="countToDB">Процент правильных ответов</param>
        /// <returns>Возврат на главную страницу</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThrowData(int id, string countToDB)
        {
            _user = GetUser();
            if (_user == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            try
            {
                bool canWrite = !ModelState.IsValid;

                // Если процент правильных ответов - пустая строка или null, то
                canWrite = !string.IsNullOrEmpty(countToDB);

                int count = Int32.Parse(countToDB);

                // Если процент минимум 80 и предыдущие этапы не запретили запись
                if (count >= 80 && canWrite == true)
                {
                    var header = await _context.TestHeaders.FirstOrDefaultAsync(x => x.Id == id);
                    //Создаем объект завершенного теста
                    CompletedTestDto completedTestDto = new CompletedTestDto()
                    {
                        TestId = id,
                        UserId = _user.Id,
                        CountByPersent = count
                    };

                    // Добавляем в БД выполненный тест
                    _context.CompletedTestsDto.Add(completedTestDto);
                    await _context.SaveChangesAsync();  // Сохраняем изменения
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Test", new { id = id });   // Возвращаемся на страницу старта теста
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            _user = GetUser();
            if (_user == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (_user.Role < 1)
            {
                return RedirectToAction("Index", "Home");
            }

            TestHeader test = null;
            List<Question> questions = new List<Question>();

            try
            {
                test = await _context.TestHeaders.FirstOrDefaultAsync(x => x.Id == id);

                if (test == null)
                {
                    throw new Exception();
                }

                questions = _context.Questions.Where(x => x.HeaderId == id).ToList();

                if (questions.Count > 0)
                {
                    foreach (var item in questions)
                    {
                        _context.Questions.Remove(item);
                    }
                }

                _context.TestHeaders.Remove(test);

                await _context.SaveChangesAsync();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
         
            return RedirectToAction("Index", "Home");
        }
    }
}
