using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        // GET: EditingController/Create
        public ActionResult CreateTest()
        {
            return View();
        }

        // POST: EditingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTest(string title, string description)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (title != null && description != null )
                    {
                        if (!title.Trim().Equals("") && !description.Trim().Equals(""))
                        {
                            TestHeader testHeader = new TestHeader()
                            {
                                Title = title.Trim(),
                                Description = description.Trim()
                            };
                            _context.TestHeaders.Add(testHeader);

                            await _context.SaveChangesAsync();
                        }
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: EditingController/Edit/5
        public ActionResult Edit(int id)
        {
            if (StaticData.Me.Role != 0)
            {
                TestHeader header = _context.TestHeaders.Where(x => x.Id == id).First();
                StaticData.ChoosedHeader = id;
                return View(header);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: EditingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string title, string description, string ask, string fakeAnswer1, string fakeAnswer2, string fakeAnswer3, string Answ)
        {
            if (StaticData.Me.Role != 0)
            {
                try
                {
                    Question questionForAdd = new Question()
                    {
                        HeaderId = StaticData.ChoosedHeader,
                        Ask = ask,
                        FakeAnswer1 = fakeAnswer1,
                        FakeAnswer2 = fakeAnswer2,
                        FakeAnswer3 = fakeAnswer3,
                        Answer = Answ
                    };
                    _context.Questions.Add(questionForAdd);

                    TestHeader header = _context.TestHeaders.Where(x => x.Id == StaticData.ChoosedHeader).FirstOrDefault();
                    
                    if (header!= null)
                    {
                        if (!title.Equals(header.Title) || !description.Equals(header.Description))
                        {
                            header.Title = title; header.Description = description;
                        }
                    }

                    _context.Update(header);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Edit", "Editing", new {id=StaticData.ChoosedHeader});
                }
                catch
                {
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
