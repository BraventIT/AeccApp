using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AeccApi.Models;
using System;
using System.Globalization;

namespace AeccApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Coordinators")]
    public class CoordinatorsController : Controller
    {
        private readonly AeccContext _context;

        public CoordinatorsController(AeccContext context)
        {
            _context = context;
        }

        // GET: api/Coordinators
        [HttpGet]
        public IActionResult GetCoordinators(string requestSource, string province)
        {
            RequestSourceEnum requestSourceFilter;

            if (string.IsNullOrEmpty(requestSource) || !Enum.TryParse(requestSource, true, out requestSourceFilter))
            {
                return BadRequest();
            }

            var coordinators = (string.IsNullOrEmpty(province)) ?
                 _context.Coordinators
                  .Where(h => h.RequestSource == requestSourceFilter) :
                  _context.Coordinators
                  .Where(h => h.RequestSource == requestSourceFilter && h.Province.StartsWith(province, true, CultureInfo.GetCultureInfo("es-ES")));

            return Ok(coordinators);
        }

        // GET: api/Coordinators/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoordinator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coordinator = await _context.Coordinators.SingleOrDefaultAsync(m => m.ID == id);

            if (coordinator == null)
            {
                return NotFound();
            }

            return Ok(coordinator);
        }

        // PUT: api/Coordinators/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoordinator([FromRoute] int id, [FromBody] Coordinator coordinator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coordinator.ID)
            {
                return BadRequest();
            }

            _context.Entry(coordinator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoordinatorExists(id))
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

        // POST: api/Coordinators
        [HttpPost]
        public async Task<IActionResult> PostCoordinator([FromBody] Coordinator coordinator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Coordinators.Add(coordinator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoordinator", new { id = coordinator.ID }, coordinator);
        }

        // DELETE: api/Coordinators/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoordinator([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coordinator = await _context.Coordinators.SingleOrDefaultAsync(m => m.ID == id);
            if (coordinator == null)
            {
                return NotFound();
            }

            _context.Coordinators.Remove(coordinator);
            await _context.SaveChangesAsync();

            return Ok(coordinator);
        }

        private bool CoordinatorExists(int id)
        {
            return _context.Coordinators.Any(e => e.ID == id);
        }
    }
}