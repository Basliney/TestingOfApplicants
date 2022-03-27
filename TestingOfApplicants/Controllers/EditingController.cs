using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        // POST: EditingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction();
            }
            catch
            {
                return View();
            }
        }
    }
}
