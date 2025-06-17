using GymSessionApp.Data;
using GymSessionApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymSessionApp.Data;
using GymSessionApp.Models;

namespace YourAppName.Controllers
{
    public class GymSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GymSessions
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var sessions = from s in _context.GymSessions select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                sessions = sessions.Where(s => s.SessionName.Contains(searchString)
                                             || s.TrainerName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    sessions = sessions.OrderByDescending(s => s.SessionName);
                    break;
                case "Date":
                    sessions = sessions.OrderBy(s => s.StartTime);
                    break;
                case "date_desc":
                    sessions = sessions.OrderByDescending(s => s.StartTime);
                    break;
                default:
                    sessions = sessions.OrderBy(s => s.SessionName);
                    break;
            }

            return View(await sessions.AsNoTracking().ToListAsync());
        }

        // GET: GymSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymSession = await _context.GymSessions
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gymSession == null)
            {
                return NotFound();
            }

            return View(gymSession);
        }

        // GET: GymSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionName,TrainerName,StartTime,EndTime,Capacity,Description,IsActive")] GymSession gymSession)
        {
            if (ModelState.IsValid)
            {
                // Validate that end time is after start time
                if (gymSession.EndTime <= gymSession.StartTime)
                {
                    ModelState.AddModelError("EndTime", "End time must be after start time.");
                    return View(gymSession);
                }

                gymSession.CreatedDate = DateTime.Now;
                _context.Add(gymSession);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Gym session created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(gymSession);
        }

        // GET: GymSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymSession = await _context.GymSessions.FindAsync(id);
            if (gymSession == null)
            {
                return NotFound();
            }
            return View(gymSession);
        }

        // POST: GymSessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionName,TrainerName,StartTime,EndTime,Capacity,Description,IsActive,CreatedDate")] GymSession gymSession)
        {
            if (id != gymSession.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Validate that end time is after start time
                if (gymSession.EndTime <= gymSession.StartTime)
                {
                    ModelState.AddModelError("EndTime", "End time must be after start time.");
                    return View(gymSession);
                }

                try
                {
                    _context.Update(gymSession);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Gym session updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymSessionExists(gymSession.Id))
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
            return View(gymSession);
        }

        // GET: GymSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymSession = await _context.GymSessions
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gymSession == null)
            {
                return NotFound();
            }

            return View(gymSession);
        }

        // POST: GymSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymSession = await _context.GymSessions.FindAsync(id);
            if (gymSession != null)
            {
                _context.GymSessions.Remove(gymSession);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Gym session deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GymSessionExists(int id)
        {
            return _context.GymSessions.Any(e => e.Id == id);
        }
    }
}