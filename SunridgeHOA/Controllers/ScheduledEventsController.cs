using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Data;
using SunridgeHOA.Models;

namespace SunridgeHOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledEventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ScheduledEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ScheduledEvents
        [HttpGet]
        public IEnumerable<ScheduledEvent> GetScheduledEvents()
        {
            return _context.ScheduledEvents;
        }

        // GET: api/ScheduledEvents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduledEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);

            if (scheduledEvent == null)
            {
                return NotFound();
            }

            return Ok(scheduledEvent);
        }

        // PUT: api/ScheduledEvents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScheduledEvent([FromRoute] int id, [FromBody] ScheduledEvent scheduledEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != scheduledEvent.ID)
            {
                return BadRequest();
            }

            _context.Entry(scheduledEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduledEventExists(id))
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

        // POST: api/ScheduledEvents
        [HttpPost]
        public async Task<IActionResult> PostScheduledEvent([FromBody] ScheduledEvent scheduledEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ScheduledEvents.Add(scheduledEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScheduledEvent", new { id = scheduledEvent.ID }, scheduledEvent);
        }

        // DELETE: api/ScheduledEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduledEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);
            if (scheduledEvent == null)
            {
                return NotFound();
            }

            _context.ScheduledEvents.Remove(scheduledEvent);
            await _context.SaveChangesAsync();

            return Ok(scheduledEvent);
        }

        private bool ScheduledEventExists(int id)
        {
            return _context.ScheduledEvents.Any(e => e.ID == id);
        }
    }
}