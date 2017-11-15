using Aecc.Extensions;
using Aecc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Hospitals")]
    [Authorize]
    public class HospitalsController : Controller
    {
        private readonly AeccContext _context;

        public HospitalsController(AeccContext context)
        {
            _context = context;
        }

        // GET: api/Hospitals
        [HttpGet]
        public IEnumerable<Hospital> GetHospitals(string province)
        {
            return (!string.IsNullOrEmpty(province)) ?
                _context.Hospitals.Where(h => h.Province.Contains(province, StringComparison.CurrentCultureIgnoreCase)) :
                _context.Hospitals;
        }

        // GET: api/Hospitals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHospital([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hospital = await _context.Hospitals
                .Include(s => s.HospitalAssignments)
                .ThenInclude(e => e.Coordinator)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (hospital == null)
            {
                return NotFound();
            }

            return Ok(hospital);
        }

        // PUT: api/Hospitals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHospital([FromRoute] int id, [FromBody] Hospital hospital)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hospital.ID)
            {
                return BadRequest();
            }

            _context.Entry(hospital).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HospitalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hospitals
        [HttpPost]
        public async Task<IActionResult> PostHospital([FromBody] Hospital hospital)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Hospitals.Add(hospital);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHospital", new { id = hospital.ID }, hospital);
        }

        // DELETE: api/Hospitals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hospital = await _context.Hospitals.SingleOrDefaultAsync(m => m.ID == id);
            if (hospital == null)
            {
                return NotFound();
            }

            _context.Hospitals.Remove(hospital);
            await _context.SaveChangesAsync();

            return Ok(hospital);
        }

        private bool HospitalExists(int id)
        {
            return _context.Hospitals.Any(e => e.ID == id);
        }
    }
}