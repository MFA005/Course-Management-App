using Assignment2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            // Check if the "LastVisit" cookie exists
            var lastVisit = Request.Cookies["LastVisit"];
            if (lastVisit == null)
            {
                // If it's the first visit, set a cookie with the current date
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1) // Cookie will expire in one year
                };
                Response.Cookies.Append("LastVisit", DateTime.Now.ToString("MM/dd/yyyy"), cookieOptions);
            }
            return View();
        }

            public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
