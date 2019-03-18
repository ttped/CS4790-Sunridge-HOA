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
    public class OwnersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OwnersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Owners
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Owner.Include(o => o.Address).Include(o => o.CoOwner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owner
                .Include(o => o.Address)
                .Include(o => o.CoOwner)
                .FirstOrDefaultAsync(m => m.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Admin/Owners/Create
        public IActionResult Create()
        {
            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id");
            ViewData["CoOwnerId"] = new SelectList(_context.Owner, "OwnerId", "OwnerId");
            return View();
        }

        // POST: Admin/Owners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Occupation,Birthday,EmergencyContactName,EmergencyContactPhone")] Models.Owner owner, Address address)
        {
            if (ModelState.IsValid)
            {
                

                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                address.LastModifiedBy = loggedInUser.FullName;
                address.LastModifiedDate = DateTime.Now;
                _context.Add(address);

                owner.Address = address;
                owner.LastModifiedBy = loggedInUser.FullName;
                owner.LastModifiedDate = DateTime.Now;

                // Add in check for primary or co-owner

                _context.Add(owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", owner.AddressId);
            ViewData["CoOwnerId"] = new SelectList(_context.Owner, "OwnerId", "OwnerId", owner.CoOwnerId);
            return View(owner);
        }

        // GET: Admin/Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owner.Include(u => u.Address).SingleOrDefaultAsync(u => u.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }
            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", owner.AddressId);
            ViewData["CoOwnerId"] = new SelectList(_context.Owner, "OwnerId", "OwnerId", owner.CoOwnerId);
            return View(owner);
        }

        // POST: Admin/Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OwnerId,AddressId,FirstName,LastName,Occupation,Birthday,EmergencyContactName,EmergencyContactPhone")] Models.Owner owner, Address address)
        {
            if (id != owner.OwnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                    var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                    var addr = await _context.Address.SingleOrDefaultAsync(u => u.Id == owner.AddressId);
                    addr.StreetAddress = address.StreetAddress;
                    addr.City = address.City;
                    addr.State = address.State;
                    addr.Zip = address.Zip;
                    addr.LastModifiedBy = loggedInUser.FullName;
                    addr.LastModifiedDate = DateTime.Now;
                    _context.Update(addr);
                    owner.LastModifiedBy = loggedInUser.FullName;
                    owner.LastModifiedDate = DateTime.Now;
                    _context.Update(owner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.OwnerId))
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
            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", owner.AddressId);
            ViewData["CoOwnerId"] = new SelectList(_context.Owner, "OwnerId", "OwnerId", owner.CoOwnerId);
            return View(owner);
        }

        // GET: Admin/Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owner
                .Include(o => o.Address)
                .Include(o => o.CoOwner)
                .FirstOrDefaultAsync(m => m.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Admin/Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            var owner = await _context.Owner.FindAsync(id);
            owner.IsArchive = true;
            owner.LastModifiedBy = loggedInUser.FullName;
            owner.LastModifiedDate = DateTime.Now;
            //_context.Owner.Remove(owner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id)
        {
            return _context.Owner.Any(e => e.OwnerId == id);
        }
    }
}
