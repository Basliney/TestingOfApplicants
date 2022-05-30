using Microsoft.AspNetCore.Mvc;

namespace TestingOfApplicants.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult OnDetectUnauthorize()
        {
            return View();
        }
    }
}
