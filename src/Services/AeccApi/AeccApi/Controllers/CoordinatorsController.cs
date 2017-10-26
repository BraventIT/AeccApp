using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AeccApi.Models;
using System.Globalization;
using AeccApi.Extensions;

namespace AeccApi.Controllers
{
    public class CoordinatorsController : Controller
    {
        private readonly AeccContext _context;

        public CoordinatorsController(AeccContext context)
        {
            _context = context;
        }

        // GET: Coordinators
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ProvinceSortParm"] = sortOrder == "Province" ? "province_desc" : "Province";
            ViewData["CurrentFilter"] = searchString;

            var coordinators = _context.Coordinators.Select(s => s);
            if (!string.IsNullOrEmpty(searchString))
            {
                coordinators = coordinators.Where(c => c.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    coordinators = coordinators.OrderByDescending(c => c.Name); break;
                case "Province":
                    coordinators = coordinators.OrderBy(c => c.Province); break;
                case "province_desc":
                    coordinators = coordinators.OrderByDescending(c => c.Province); break;
                default:
                    coordinators = coordinators.OrderBy(c => c.Name); break;
            }

            return View(await coordinators.AsNoTracking().ToListAsync());
        }

        // GET: Coordinators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinators
                .Include(s=> s.Employments)
                .ThenInclude(e=> e.Hospital)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (coordinator == null)
            {
                return NotFound();
            }

            return View(coordinator);
        }

        // GET: Coordinators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coordinators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Telephone,Province")] Coordinator coordinator)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(coordinator);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(coordinator);
        }

        // GET: Coordinators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinators.SingleOrDefaultAsync(m => m.ID == id);
            if (coordinator == null)
            {
                return NotFound();
            }
            return View(coordinator);
        }

        // POST: Coordinators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Email,Telephone,Province")] Coordinator coordinator)
        {
            if (id != coordinator.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coordinator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coordinator);
        }

        // GET: Coordinators/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError=false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinators
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (coordinator == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(coordinator);
        }

        // POST: Coordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Coordinator coordinatorToDelete = new Coordinator() { ID = id };
                _context.Entry(coordinatorToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool CoordinatorExists(int id)
        {
            return _context.Coordinators.Any(e => e.ID == id);
        }
    }
}
