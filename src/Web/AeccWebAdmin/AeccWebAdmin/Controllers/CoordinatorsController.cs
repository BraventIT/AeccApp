using Aecc.Models;
using AeccApi.WebAdmin.Extensions;
using AeccApi.WebAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApi.WebAdmin.Controllers
{
#if !DEBUG
    [Authorize]
#endif
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
                .Include(s=> s.HospitalAssignments)
                .ThenInclude(e=> e.Hospital)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (coordinator == null)
            {
                return NotFound();
            }

            return View(coordinator);
        }

        private void PopulateAssignedHospitalVM(Coordinator coordinator)
        {
            var provinceHospitals = _context.Hospitals.Where(h => 
                !string.IsNullOrEmpty( h.Name)
                && h.Province.Contains(coordinator.Province, StringComparison.CurrentCultureIgnoreCase));
            var coordinatorHospitals = new HashSet<int>(coordinator.HospitalAssignments.Select(c => c.HospitalID));
            var viewModel = new List<AssignmentHospitalData>();

            foreach (var hospitals in provinceHospitals)
            {
                viewModel.Add(new AssignmentHospitalData
                {
                    HospitalID = hospitals.ID,
                    Name = hospitals.Name,
                    Assigned = coordinatorHospitals.Contains(hospitals.ID)
                });
            }
            ViewData["Hospitals"] = viewModel;
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
        public async Task<IActionResult> Create([Bind("Name,Email,Telephone,Province,RequestSource")] Coordinator coordinator)
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

            var coordinator = await _context.Coordinators
                .Include(c=> c.HospitalAssignments)
                .ThenInclude(c => c.Hospital)
                .AsTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (coordinator == null)
            {
                return NotFound();
            }

            PopulateAssignedHospitalVM(coordinator);
            return View(coordinator);
        }

        // POST: Coordinators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedHospitals)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinatorToUpdate = await _context.Coordinators
                .Include(c => c.HospitalAssignments)
                .ThenInclude(c=> c.Hospital)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync(coordinatorToUpdate,
                "",
                i=> i.Name, i=> i.Email, i=> i.Telephone, i=> i.Province, i=> i.RequestSource))
            {
                UpdateAssignedHospitals(selectedHospitals, coordinatorToUpdate);
                try
                {
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
            UpdateAssignedHospitals(selectedHospitals, coordinatorToUpdate);
            PopulateAssignedHospitalVM(coordinatorToUpdate);
            return View(coordinatorToUpdate);
        }

        private void UpdateAssignedHospitals(string[] selectedHospitals, Coordinator coordinatorToUpdate)
        {
            if (selectedHospitals==null)
            {
                coordinatorToUpdate.HospitalAssignments = new List<HospitalAssignment>();
                return;
            }

            var selectedHospitalsHS = new HashSet<string>(selectedHospitals);
            var coordinatorHospitals = new HashSet<int>
                (coordinatorToUpdate.HospitalAssignments.Select(h => h.HospitalID));


            foreach (var hospital in _context.Hospitals)
            {
                if (selectedHospitalsHS.Contains(hospital.ID.ToString()) )
                {
                    if (!coordinatorHospitals.Contains(hospital.ID))
                    {
                        coordinatorToUpdate.HospitalAssignments.Add(new HospitalAssignment { CoordinatorID = coordinatorToUpdate.ID, HospitalID = hospital.ID });
                    }
                }
                else
                {
                    if (coordinatorHospitals.Contains(hospital.ID))
                    {
                        var hospitalToRemove = coordinatorToUpdate.HospitalAssignments.SingleOrDefault(c => c.HospitalID == hospital.ID);
                        coordinatorToUpdate.HospitalAssignments.Remove(hospitalToRemove);
                    }
                }
            }
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
