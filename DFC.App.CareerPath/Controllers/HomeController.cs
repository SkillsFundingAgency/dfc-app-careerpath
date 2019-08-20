using DFC.App.CareerPath.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DFC.App.CareerPath.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
