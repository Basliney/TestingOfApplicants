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

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public TestController(ApplicationContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Обработка Get запроса
        /// </summary>
        /// <param name="id">Номер теста</param>
        /// <returns>Вид</returns>
        public async Task<ActionResult> Index(int id)
        {
            // Если Пользователь еще не прошел этот тест
            if (await _context.CompletedTestsDto.FirstOrDefaultAsync(x => (x.TestId == id && x.UserId == StaticData.Me.Id)) == null)
            {
                List<Question> questionList = new List<Question>(); // Объявляем список вопросов
                questionList.AddRange(_context.Questions.Where(x => x.HeaderId == id)); // Заносим в список вопросы этого теста
                
                // Если вопросов минимум 5
                if (questionList.Count >= 5)
                {
                    StaticData.ChoosedHeader = id;  //Запоминаем id теста
                    StaticData.header = _context.TestHeaders.Where(x => x.Id == id).FirstOrDefault();   // Запоминаем шапку теста
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
        public async Task<ActionResult> ThrowData(string countToDB)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Если процент правильных ответов не пустая строка и не null, то
                    if (!countToDB.Equals("") && countToDB != null)
                    {
                        // Парсим из текста число
                        int count = Int32.Parse(countToDB);

                        // Если процент минимум 80
                        if (count >= 80)
                        {
                            //Создаем объект завершенного теста
                            CompletedTestDto completedTestDto = new CompletedTestDto()
                            {
                                TestId = StaticData.ChoosedHeader,
                                UserId = StaticData.Me.Id,
                                CountByPersent = count
                            };

                            // Добавляем в БД выполненный тест
                            _context.CompletedTestsDto.Add(completedTestDto);
                            await _context.SaveChangesAsync();  // Сохраняем изменения
                        }
                    }
                }
                return RedirectToAction("Index", "Home");   // Возвращаемся на главную
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");   // Возвращаемся на главную
            }
        }
    }
}
