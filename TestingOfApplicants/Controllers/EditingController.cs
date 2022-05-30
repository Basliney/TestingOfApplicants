using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestingOfApplicants.Models;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Controllers
{
    public class EditingController : Controller
    {
        ApplicationContext _context;

        public EditingController(ApplicationContext context)
        {
            _context = context;
        }

        public ActionResult CreateTest()
        {
            if (StaticData.Me == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (StaticData.Me.Role < 1)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTest(int theme, string title, string description)
        {
            try
            {
                if (StaticData.Me == null)
                {
                    return RedirectToAction("Login", "Authorization");
                }

                if (!ModelState.IsValid || StaticData.Me.Role < 1)
                {
                    return RedirectToAction("Index", "Home");
                }

                try
                {
                    title = title.Trim();
                    description = description.Trim();
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
                {
                    return View();
                }

                TestHeader testHeader = new TestHeader()
                {
                    Title = title,
                    Description = description,
                    Subjectid = theme
                };
                _context.TestHeaders.Add(testHeader);

                await _context.SaveChangesAsync();

                ViewBag.selectedSubject = theme;

                return RedirectToAction("Edit", "Editing", 
                    new {id = _context.TestHeaders.FirstOrDefault(x=>x.Title.Equals(title)).Id});
            }
            catch
            {
                return View();
            }
        }

        public async Task <IActionResult> Edit(int id)
        {
            if (StaticData.Me == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (StaticData.Me.Role < 1)
            {
                return RedirectToAction("Index", "Home");
            }

            TestHeader header = await _context.TestHeaders.FirstOrDefaultAsync(x => x.Id == id);

            if (header == null)
            {
                return RedirectToAction("Index", "Home");
            }

            StaticData.ChoosedHeader = id;
            ViewBag.selectedSubject = header.Subjectid;
            return View(header);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int theme, string title, string description, string ask, string fakeAnswer1, string fakeAnswer2, string fakeAnswer3, string Answ)
        {
            if (StaticData.Me.Role < 1)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                if (!string.IsNullOrEmpty(ask)
                    && !string.IsNullOrEmpty(fakeAnswer1)
                    && !string.IsNullOrEmpty(fakeAnswer2)
                    && !string.IsNullOrEmpty(fakeAnswer3)
                    && !string.IsNullOrEmpty(Answ))
                {
                    Question questionForAdd = new Question()
                    {
                        HeaderId = StaticData.ChoosedHeader,
                        Ask = ask,
                        FakeAnswer1 = fakeAnswer1,
                        FakeAnswer2 = fakeAnswer2,
                        FakeAnswer3 = fakeAnswer3,
                        Answer = Answ,
                    };
                    _context.Questions.Add(questionForAdd);
                }

                TestHeader header = _context.TestHeaders.Where(x => x.Id == StaticData.ChoosedHeader).FirstOrDefault();

                if (header != null)
                {
                    if (!title.Equals(header.Title) || !description.Equals(header.Description) || header.Subjectid != theme)
                    {
                        header.Title = title; header.Description = description;
                        header.Subjectid = theme;
                    }
                }

                _context.Update(header);
                await _context.SaveChangesAsync();
                ViewBag.selectedSubject = theme;
                return RedirectToAction("Edit", "Editing", new { id = StaticData.ChoosedHeader });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> QuestionDetails(int id)
        {
            if (StaticData.Me == null)
            {
                return RedirectToAction("Login", "Authorization");
            }
            if (StaticData.Me.Role < 1)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == id);
                var header = await _context.TestHeaders.FirstOrDefaultAsync(x => x.Id == question.HeaderId);
                ViewBag.Question = question;

                StaticData.QuestionOnEdit = new Question
                {
                    Ask = question.Ask,
                    Id = question.Id,
                    FakeAnswer1 = question.FakeAnswer1,
                    FakeAnswer2 = question.FakeAnswer2,
                    FakeAnswer3 = question.FakeAnswer3,
                    Answer = question.Answer,
                    HeaderId = question.HeaderId
                };

                ViewBag.Header = header;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuestion(string ask, string fakeAnswer1, string fakeAnswer2, string fakeAnswer3, string answ)
        {
            if (StaticData.Me == null)
            {
                return RedirectToAction("Login", "Authorization");
            }
            if (StaticData.Me.Role < 1)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                Question question = StaticData.QuestionOnEdit;

                question.Ask = ask;
                question.FakeAnswer1 = fakeAnswer1;
                question.FakeAnswer2 = fakeAnswer2;
                question.FakeAnswer3 = fakeAnswer3;
                question.Answer = answ;

                _context.Questions.Update(question);
                await _context.SaveChangesAsync();

                StaticData.QuestionOnEdit = null;
                return RedirectToAction("TestDetails", "Statistics");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion()
        {
            if (StaticData.Me == null)
            {
                return RedirectToAction("Login", "Authorization");
            }
            if (StaticData.Me.Role < 1)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                _context.Questions.Remove(StaticData.QuestionOnEdit);
                await _context.SaveChangesAsync();
                StaticData.QuestionOnEdit = null;

                return RedirectToAction("TestDetails", "Statistics");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
