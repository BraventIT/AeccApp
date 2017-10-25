using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AeccApi.Models;

namespace AeccApi.Controllers.API
{
    [Produces("application/json")]
    [Route("api/RequestTypes")]
    public class RequestTypesController : Controller
    {
        private readonly AeccContext _context;

        public RequestTypesController(AeccContext context)
        {
            _context = context;
        }

        // GET: api/RequestTypes
        [HttpGet]
        public IEnumerable<RequestType> GetRequestTypes(string requestSource)
        {
            RequestSourceEnum requestSourceFilter;

            return (!string.IsNullOrEmpty(requestSource) && Enum.TryParse(requestSource, true, out requestSourceFilter)) ?
                 _context.RequestTypes.Where(r => r.Source == requestSourceFilter) : _context.RequestTypes;
        }

        // GET: api/RequestTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requestType = await _context.RequestTypes.SingleOrDefaultAsync(m => m.Id == id);

            if (requestType == null)
            {
                return NotFound();
            }

            return Ok(requestType);
        }

        // PUT: api/RequestTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestType([FromRoute] int id, [FromBody] RequestType requestType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != requestType.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestTypeExists(id))
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

        // POST: api/RequestTypes
        [HttpPost]
        public async Task<IActionResult> PostRequestType([FromBody] RequestType requestType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.RequestTypes.Add(requestType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequestType", new { id = requestType.Id }, requestType);
        }

        // DELETE: api/RequestTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requestType = await _context.RequestTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (requestType == null)
            {
                return NotFound();
            }

            _context.RequestTypes.Remove(requestType);
            await _context.SaveChangesAsync();

            return Ok(requestType);
        }

        private bool RequestTypeExists(int id)
        {
            return _context.RequestTypes.Any(e => e.Id == id);
        }
    }
}