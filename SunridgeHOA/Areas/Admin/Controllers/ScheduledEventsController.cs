using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Data;
using SunridgeHOA.Models;
using SunridgeHOA.Utility;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticData.AdminEndUser)]
    [Area("Admin")]
    public class ScheduledEventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public ScheduledEventsController(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: SuperAdmin/ScheduledEvents
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScheduledEvents.ToListAsync());
        }

        // GET: SuperAdmin/ScheduledEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledEvent = await _context.ScheduledEvents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (scheduledEvent == null)
            {
                return NotFound();
            }

            return View(scheduledEvent);
        }

        // GET: SuperAdmin/ScheduledEvents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SuperAdmin/ScheduledEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Subject,Description,Start,End,IsFullDay,Location")] ScheduledEvent scheduledEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduledEvent);
                await _context.SaveChangesAsync();

                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var eventFromDB = _context.ScheduledEvents.Find(scheduledEvent.ID);

                if (files.Count != 0)
                {
                    var uploads = Path.Combine(webRootPath, StaticData.EventImagesPath);
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(uploads, eventFromDB.ID + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    eventFromDB.Image = @"\" + StaticData.EventImagesPath + @"\" + scheduledEvent.ID + extension;

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(scheduledEvent);
        }

        // GET: SuperAdmin/ScheduledEvents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);
            if (scheduledEvent == null)
            {
                return NotFound();
            }
            return View(scheduledEvent);
        }

        // POST: SuperAdmin/ScheduledEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Subject,Description,Start,End,IsFullDay,Location,Image")] ScheduledEvent scheduledEvent)
        {
            if (id != scheduledEvent.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;

                    if (files.Count > 0 && files[0] != null)
                    {
                        var uploads = Path.Combine(webRootPath, StaticData.EventImagesPath);
                        var extensionNew = Path.GetExtension(files[0].FileName);
                        var extensionOld = Path.GetExtension(scheduledEvent.Image);

                        if (System.IO.File.Exists(Path.Combine(uploads, scheduledEvent.ID + extensionOld)))
                        {
                            System.IO.File.Delete(Path.Combine(uploads, scheduledEvent.ID + extensionOld));
                        }

                        using (var filestream = new FileStream(Path.Combine(uploads, scheduledEvent.ID + extensionNew), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }

                        scheduledEvent.Image = @"\" + StaticData.EventImagesPath + @"\" + scheduledEvent.ID +
                                               extensionNew;
                    }



                    _context.Update(scheduledEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduledEventExists(scheduledEvent.ID))
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
            return View(scheduledEvent);
        }

        // GET: SuperAdmin/ScheduledEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledEvent = await _context.ScheduledEvents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (scheduledEvent == null)
            {
                return NotFound();
            }

            return View(scheduledEvent);
        }

        // POST: SuperAdmin/ScheduledEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);

            if (scheduledEvent == null)
            {
                return NotFound();
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            var uploads = Path.Combine(webRootPath, StaticData.EventImagesPath);
            var extension = Path.GetExtension(scheduledEvent.Image);

            if (System.IO.File.Exists(Path.Combine(uploads, scheduledEvent.ID + extension)))
            {
                System.IO.File.Delete(Path.Combine(uploads, scheduledEvent.ID + extension));
            }

            _context.ScheduledEvents.Remove(scheduledEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduledEventExists(int id)
        {
            return _context.ScheduledEvents.Any(e => e.ID == id);
        }
    }
}