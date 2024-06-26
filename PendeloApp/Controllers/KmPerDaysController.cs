﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PendeloApp.Data;
using PendeloApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PendeloApp.Controllers
{
    [Authorize] // Require authentication for this controller
    public class KmPerDaysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public KmPerDaysController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: KmPerDays
        public async Task<IActionResult> Index()
        {
            // Get the current user
            var currentUser = await _userManager.GetUserAsync(User);

            var kmPerDays = await _context.KmPerDay
                .Where(k => k.UserID == currentUser.Id)
                .Include(k => k.User)
                .ToListAsync();

            return View(kmPerDays);
        }

        public async Task<IActionResult> UserKM()
        {
            var kmPerDayList = await _context.KmPerDay
                                             .Include(k => k.User)
                                             .ToListAsync();
            return View(kmPerDayList);
        }
        //Get Get LeaderBorad of KmPerDay by User
       

        // GET: KmPerDays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kmPerDay = await _context.KmPerDay
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (kmPerDay == null)
            {
                return NotFound();
            }

            return View(kmPerDay);
        }
        public async Task<IActionResult> LeaderBoard()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var leaderboard = await _context.KmPerDay
                .GroupBy(k => k.User)
                .Select(g => new
                {
                    User = g.Key,
                    TotalKilometers = g.Sum(k => k.Kilometers)
                })
                .OrderByDescending(x => x.TotalKilometers)
                .ToListAsync();

            var leaderboardViewModel = leaderboard.Select(x => new LeaderboardViewModel
            {
                UserName = x.User.UserName,
                TotalKilometers = x.TotalKilometers
            }).ToList();

            var currentUserEntry = leaderboardViewModel.FirstOrDefault(x => x.UserName == currentUser.UserName);
            ViewBag.CurrentUserKilometers = currentUserEntry?.TotalKilometers;

            return View(leaderboardViewModel);
        }

        // GET: KmPerDays/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: KmPerDays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DriveDate,Kilometers")] KmPerDay kmPerDay)
        {

                var currentUser = await _userManager.GetUserAsync(User);
                kmPerDay.UserID = currentUser.Id;

                _context.Add(kmPerDay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

        }


        // GET: KmPerDays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kmPerDay = await _context.KmPerDay.FindAsync(id);
            if (kmPerDay == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", kmPerDay.UserID);
            return View(kmPerDay);
        }

        // POST: KmPerDays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,DriveDate,Kilometers")] KmPerDay kmPerDay)
        {
                try
                {
                    _context.Update(kmPerDay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KmPerDayExists(kmPerDay.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));


        }

        // GET: KmPerDays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kmPerDay = await _context.KmPerDay
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (kmPerDay == null)
            {
                return NotFound();
            }

            return View(kmPerDay);
        }

        // POST: KmPerDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kmPerDay = await _context.KmPerDay.FindAsync(id);
            if (kmPerDay != null)
            {
                _context.KmPerDay.Remove(kmPerDay);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KmPerDayExists(int id)
        {
            return _context.KmPerDay.Any(e => e.ID == id);
        }
    }
}
