using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InventoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Inventories
        public async Task<IActionResult> Index()
        {
            var inventories = await _context.Inventory.Where(i => i.IsArchive == false).ToListAsync();

            //return View(await _context.Inventory.ToListAsync());
            return View(inventories);

        }

        // GET: Admin/Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Admin/Inventories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Inventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventoryId,Description,IsArchive,LastModifiedBy,LastModifiedDate")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                inventory.IsArchive = false;
                inventory.LastModifiedBy = loggedInUser.FullName;
                inventory.LastModifiedDate = DateTime.Now;

                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        // GET: Admin/Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        // POST: Admin/Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryId,Description,IsArchive,LastModifiedBy,LastModifiedDate")] Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                    var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                    inventory.LastModifiedBy = loggedInUser.FullName;
                    inventory.LastModifiedDate = DateTime.Now;

                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.InventoryId))
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
            return View(inventory);
        }

        // GET: Admin/Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Admin/Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            //_context.Inventory.Remove(inventory);

            inventory.IsArchive = true;
            inventory.LastModifiedBy = loggedInUser.FullName;
            inventory.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.InventoryId == id);
        }
    }
}
