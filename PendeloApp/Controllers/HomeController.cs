using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PendeloApp.Data;
using PendeloApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PendeloApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var topUser = await _context.KmPerDay
               .GroupBy(k => k.User)
               .Select(g => new
               {
                   UserName = g.Key.UserName,
                   TotalKilometers = g.Sum(k => k.Kilometers)
               })
               .OrderByDescending(x => x.TotalKilometers)
               .FirstOrDefaultAsync();

            ViewBag.TopUser = topUser?.UserName;
            ViewBag.TopUserKilometers = topUser?.TotalKilometers;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TestProfilex()
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
