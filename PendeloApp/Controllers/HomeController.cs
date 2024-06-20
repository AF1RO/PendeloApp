using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PendeloApp.Data;
using PendeloApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PendeloApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
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

        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> ShowUsers()
        {
            var users = _context.Users.ToList();
            var userCount = users.Count();
            var bikeCount = _context.Bike.Count();

            var statistics = new AdminStatisticsViewModel
            {
                UserCount = userCount,
                BikeCount = bikeCount
            };

            var model = new Tuple<IEnumerable<ApplicationUser>, AdminStatisticsViewModel>(users, statistics);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
