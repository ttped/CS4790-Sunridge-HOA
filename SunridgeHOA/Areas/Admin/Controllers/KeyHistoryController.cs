using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;
using SunridgeHOA.Models.ViewModels;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class KeyHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public KeyHistoryViewModel keyHistoryViewModel { get; set; }


        public KeyHistoryController(ApplicationDbContext context, HostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            keyHistoryViewModel = new KeyHistoryViewModel()
            {
                KeyHistory = new SunridgeHOA.Models.KeyHistory(),
                Key = _context.Key.ToList(),
                Lot = _context.Lot.ToList()
            };
        }

        public async Task<ActionResult> Index(string searchString)
        {
            IQueryable<KeyHistory> KeySearch = from c in _context.KeyHistory
                                               select c;

            if (!String.IsNullOrEmpty(searchString))
            {

                KeySearch = KeySearch.Where(c => //c.Status.Contains(searchString) ||
                //c.PaidAmount.ToString().Contains(searchString) ||
                c.Key.SerialNumber.Contains(searchString) ||
                c.Lot.LotNumber.Contains(searchString)// ||
                //c.LastModifiedBy.Contains(searchString) ||
                //c.DateIssued.ToString("mm/dd/yyyy 0:HH:mm:ss tt").Contains(searchString) ||
                //c.DateReturned?.ToString("mm/dd/yyyy 0:HH:mm:ss tt").Contains(searchString) ||
                //c.LastModifiedDate.ToString("mm/dd/yyyy 0:HH:mm:ss tt").Contains(searchString)
                );
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

            var keyHistory = await _context.KeyHistory.Include(u => u.Lot).SingleOrDefaultAsync(m => m.KeyHistoryId == id);
            if (keyHistory == null)
            {
                return NotFound();
            }

            return View(keyHistory);
        }

        // GET: Key/Create
        public ActionResult Create()
        {
            ViewData["LotsSelect"] = new SelectList(_context.Lot.OrderBy(u => u.LotNumber), "LotId", "LotNumber");
            return View(keyHistoryViewModel);
        }

        // POST: Key/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(KeyHistory keyHistory)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                keyHistory.Status = "Active";
                keyHistory.IsArchive = false;
                keyHistory.LastModifiedBy = loggedInUser.FullName;
                keyHistory.LastModifiedDate = DateTime.Now;
                

                _context.KeyHistory.Add(keyHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["LotsSelect"] = new SelectList(_context.Lot.OrderBy(u => u.LotNumber), "LotId", "LotNumber", keyHistory.LotId);
            return View();
        }

        // GET: Key/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.KeyHistory.FindAsync(id);
            keyHistoryViewModel.KeyHistory = item;
            if (item == null)
            {
                return NotFound();
            }

            ViewData["LotsSelect"] = new SelectList(_context.Lot.OrderBy(u => u.LotNumber), "LotId", "LotNumber", item.LotId);
            return View(keyHistoryViewModel);
        }

        // POST: Key/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, KeyHistory keyHistory)
        {
            if (id != keyHistory.KeyHistoryId)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            
            keyHistory.LastModifiedBy = loggedInUser.FullName;
            keyHistory.LastModifiedDate = DateTime.Now;

            _context.Update(keyHistory);
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

            var item = await _context.KeyHistory.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Key/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, KeyHistory keyHistory)
        {
            KeyHistory item = await _context.KeyHistory.FindAsync(id);
            item.IsArchive = true;
            _context.KeyHistory.Update(item);
            //_context.KeyHistory.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}