using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class KeysController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;


        public KeysController(ApplicationDbContext context, HostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }
        // GET: Key
        public async Task<ActionResult> Index(string searchString)
        {
            IQueryable<Key> KeySearch = from c in _context.Key
                                            select c;
            
            if (!String.IsNullOrEmpty(searchString))
            {

                KeySearch = KeySearch.Where(c => c.FullSerial.Contains(searchString));
            }


            return View(await KeySearch.ToListAsync());
        }

        // GET: Key/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            var key = await _context.Key.SingleOrDefaultAsync(m => m.KeyId == id);
            if (key == null)
            {
                return NotFound();
            }

            return View(key);
        }

        // GET: Key/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Key/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Key key)
        {
            int testNum;
            if (!Int32.TryParse(key.SerialNumber, out testNum))
            {
                ModelState.AddModelError("SerialNumber", "Serial number can only contain digits 0-9");
            }

            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                key.IsArchive = false;
                key.LastModifiedBy = loggedInUser.FullName;
                key.LastModifiedDate = DateTime.Now;

                _context.Key.Add(key);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Key/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Key.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Key/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Key key)
        {
            if (id != key.KeyId)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            key.LastModifiedBy = loggedInUser.FullName;
            key.LastModifiedDate = DateTime.Now;

            _context.Update(key);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Key/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Key.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Key/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> Delete(int id, Key key)
        {
            Key item = await _context.Key.FindAsync(id);
            item.IsArchive = true;
            _context.Key.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}