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
        private User _user = null;

        public EditingController(ApplicationContext context)
        {
            _context = context;
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

        [HttpGet]
        public ActionResult CreateTest()
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

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTest(int theme, string title, string description)
        {
            _user = GetUser();

            if (_user == null)
            {
                return RedirectToAction("Login", "Authorization");
            }

            if (!ModelState.IsValid || _user.Role < 1)
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

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(theme.ToString()))
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
                new { id = _context.TestHeaders.FirstOrDefault(x => x.Title.Equals(title)).Id });
        }

        public async Task <IActionResult> Edit(int id)
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

            TestHeader header = await _context.TestHeaders.FirstOrDefaultAsync(x => x.Id == id);

            if (header == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ChoosedHeader = id;
            ViewBag.selectedSubject = header.Subjectid;
            return View(header);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, int theme, string title, string description, string ask, string fakeAnswer1, string fakeAnswer2, string fakeAnswer3, string Answ)
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

            try
            {
                var header = await _context.TestHeaders.FirstOrDefaultAsync(x => x.Id == id);

                if (!string.IsNullOrEmpty(ask.Trim())
                    && !string.IsNullOrEmpty(fakeAnswer1.Trim())
                    && !string.IsNullOrEmpty(fakeAnswer2.Trim())
                    && !string.IsNullOrEmpty(fakeAnswer3.Trim())
                    && !string.IsNullOrEmpty(Answ.Trim()))
                {
                    Question questionForAdd = new Question()
                    {
                        HeaderId = header.Id,
                        Ask = ask,
                        FakeAnswer1 = fakeAnswer1,
                        FakeAnswer2 = fakeAnswer2,
                        FakeAnswer3 = fakeAnswer3,
                        Answer = Answ,
                    };
                    _context.Questions.Add(questionForAdd);
                }

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
                return RedirectToAction("Edit", "Editing", new { id = header.Id });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> QuestionDetails(int id)
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

            try
            {
                var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == id);
                var header = await _context.TestHeaders.FirstOrDefaultAsync(x => x.Id == question.HeaderId);
                ViewBag.Question = question;

                ViewBag.QuestionOnEdit = new Question
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
        public async Task<IActionResult> UpdateQuestion(int id, string ask, string fakeAnswer1, string fakeAnswer2, string fakeAnswer3, string answ)
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

            try
            {
                Question question = null;
                question = await _context.Questions.FirstOrDefaultAsync(x=>x.Id == id);

                question.Ask = ask;
                question.FakeAnswer1 = fakeAnswer1;
                question.FakeAnswer2 = fakeAnswer2;
                question.FakeAnswer3 = fakeAnswer3;
                question.Answer = answ;

                _context.Questions.Update(question);
                _context.SaveChanges();

                return RedirectToAction("TestDetails", "Statistics", new { id = question.HeaderId });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(int id)
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

            try
            {
                var question = await _context.Questions.FirstOrDefaultAsync(x=>x.Id == id);
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();

                return RedirectToAction("TestDetails", "Statistics", new {id = question.HeaderId});
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
