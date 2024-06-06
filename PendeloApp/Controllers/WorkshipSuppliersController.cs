using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PendeloApp.Data;
using PendeloApp.Models;

namespace PendeloApp.Controllers
{
    [Authorize]
    public class WorkshipSuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public WorkshipSuppliersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: WorkshipSuppliers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WorkshipSupplier.Include(w => w.User);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Map()
        {
            return View();
        }

        //GET: WorkshipSuppliers/GetNearby
        [HttpGet]
        public IActionResult GetNearby(double lat, double lng, double radius)
        {
            var suppliers = _context.WorkshipSupplier.ToList();
            var nearbySuppliers = suppliers.Where(s => GetDistance(s.Latitude, s.Longitude, lat, lng) <= radius).ToList();

            return Json(nearbySuppliers);
        }

        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius der Erde in km
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;
            return distance;
        }

        // GET: WorkshipSuppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshipSupplier = await _context.WorkshipSupplier
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (workshipSupplier == null)
            {
                return NotFound();
            }

            return View(workshipSupplier);
        }

        // GET: WorkshipSuppliers/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: WorkshipSuppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,CompanyName,Services,Latitude,Longitude,Email,Phone")] WorkshipSupplier workshipSupplier)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            workshipSupplier.UserID = currentUser.Id;
            workshipSupplier.Latitude =  Convert.ToDouble(Request.Form["Latitude"], CultureInfo.InvariantCulture);
            workshipSupplier.Longitude = Convert.ToDouble(Request.Form["Longitude"], CultureInfo.InvariantCulture);

            _context.Add(workshipSupplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: WorkshipSuppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshipSupplier = await _context.WorkshipSupplier.FindAsync(id);
            if (workshipSupplier == null)
            {
                return NotFound();
            }

            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", workshipSupplier.UserID);
            return View(workshipSupplier);
        }

        // POST: WorkshipSuppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,CompanyName,Services,Latitude,Longitude,Email,Phone")] WorkshipSupplier workshipSupplier)
        {
            try
            {
                workshipSupplier.Latitude = Convert.ToDouble(Request.Form["Latitude"], CultureInfo.InvariantCulture);
                workshipSupplier.Longitude = Convert.ToDouble(Request.Form["Longitude"], CultureInfo.InvariantCulture);
                _context.Update(workshipSupplier);
               
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkshipSupplierExists(workshipSupplier.ID))
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

        // GET: WorkshipSuppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshipSupplier = await _context.WorkshipSupplier
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (workshipSupplier == null)
            {
                return NotFound();
            }

            return View(workshipSupplier);
        }

        // POST: WorkshipSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workshipSupplier = await _context.WorkshipSupplier.FindAsync(id);
            if (workshipSupplier != null)
            {
                _context.WorkshipSupplier.Remove(workshipSupplier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkshipSupplierExists(int id)
        {
            return _context.WorkshipSupplier.Any(e => e.ID == id);
        }
    }
}
