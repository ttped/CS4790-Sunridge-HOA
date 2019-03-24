using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Areas.Owner.Models;
using SunridgeHOA.Models;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Owner")]
    public class LotsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LotsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Lots
        public async Task<IActionResult> Index()
        {
            var lots = await _context.Lot
                .Include(l => l.Address)
                .OrderBy(u => u.LotNumber)
                .ToListAsync();

            var vm = new List<LotIndexVM>();

            foreach (var lot in lots)
            {
                var owners = _context.OwnerLot
                    .Include(u => u.Owner)
                    .Where(u => u.LotId == lot.LotId)
                    .Where(u => u.EndDate == DateTime.MinValue);

                var lotItems = await _context.LotInventory
                    .Include(u => u.Inventory)
                    .Where(u => u.LotId == lot.LotId)
                    .Select(u => u.Inventory)
                    .ToListAsync();


                if (!owners.Any())
                {
                    vm.Add(new LotIndexVM
                    {
                        Lot = lot,
                        Address = lot.Address,
                        PrimaryOwner = null,
                        Owners = null,
                        InventoryItems = lotItems
                    });
                }
                else
                {
                    vm.Add(new LotIndexVM
                    {
                        Lot = lot,
                        Address = lot.Address,
                        PrimaryOwner = owners.Where(u => u.IsPrimary).First().Owner,
                        Owners = owners.Where(u => !u.IsPrimary).Select(u => u.Owner).ToList(),
                        InventoryItems = lotItems
                    });
                }

                
            }
            return View(vm);
        }

        // GET: Admin/Lots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lot
                .Include(l => l.Address)
                .FirstOrDefaultAsync(m => m.LotId == id);
            if (lot == null)
            {
                return NotFound();
            }

            var lotItems = await _context.LotInventory
                    .Include(u => u.Inventory)
                    .Where(u => u.LotId == lot.LotId)
                    .Select(u => u.Inventory)
                    .ToListAsync();

            var vm = new LotIndexVM
            {
                Lot = lot,
                Address = lot.Address,
                PrimaryOwner = await GetPrimaryOwnerAsync(lot.LotId),
                Owners = await GetNonPrimaryOwnerAsync(lot.LotId),
                InventoryItems = lotItems
            };

            return View(vm);
        }

        // GET: Admin/Lots/Create
        public IActionResult Create()
        {
            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id");
            return View();
        }

        // POST: Admin/Lots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LotEditVM vm)
        {
            if (_context.Lot.Where(u => u.LotNumber == vm.Lot.LotNumber).Any())
            {
                ModelState.AddModelError("LotNumber", "A lot already exists with that lot number");
            }
            if (_context.Lot.Where(u => u.TaxId == vm.Lot.TaxId).Any())
            {
                ModelState.AddModelError("TaxId", "A lot already exists with that tax id");
            }

            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                // We can auto fill these details because all lots will have same city/state/zip
                var address = new Address
                {
                    StreetAddress = vm.Address.StreetAddress,
                    City = "Some City",
                    State = "Utah",
                    Zip = "12345",
                    LastModifiedBy = loggedInUser.FullName,
                    LastModifiedDate = DateTime.Now
                };
                _context.Add(address);
                await _context.SaveChangesAsync();

                var lot = new Lot
                {
                    LotNumber = vm.Lot.LotNumber,
                    TaxId = vm.Lot.TaxId,
                    Address = address,
                    LastModifiedBy = loggedInUser.FullName,
                    LastModifiedDate = DateTime.Now
                };
                _context.Add(lot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", lot.AddressId);
            return View(vm);
        }

        // GET: Admin/Lots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lot.Include(u => u.Address).SingleOrDefaultAsync(u => u.LotId == id);
            if (lot == null)
            {
                return NotFound();
            }

            var currPrimary = await GetPrimaryOwnerAsync(id.Value);
            var currOwnerIds = await _context.OwnerLot
                .Include(u => u.Owner)
                .Where(u => u.LotId == id)
                .Where(u => u.EndDate == DateTime.MinValue)
                .Select(u => u.Owner.OwnerId)
                .ToListAsync();

            //ViewData["Owner"] = new SelectList(_context.Owner, "OwnerId", "FullName", currPrimary.OwnerId);
            ViewData["OwnerList"] = _context.Owner.ToList();

            var vm = new LotEditVM
            {
                Lot = lot,
                Address = lot.Address,
                OwnerId = currPrimary?.OwnerId ?? -1,
                OwnerIds = currOwnerIds
            };
            return View(vm);
        }

        // POST: Admin/Lots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LotEditVM vm)
        {
            if (id != vm.Lot.LotId)
            {
                return NotFound();
            }

            //if (vm.OwnerId == -1)
            //{
            //    // Assuming that a lot must always have an owner
            //    ModelState.AddModelError("OwnerId", "You must select an owner");
            //}
            if (vm.OwnerIds != null && !vm.OwnerIds.Contains(vm.OwnerId))
            {
                // This shouldn't come up, BUT JUST IN CASE
                ModelState.AddModelError("OwnerIds", "List of owners must contain the primary owner");
            }

            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                try
                {
                    // Update Address entry
                    var addr = _context.Address.Find(vm.Address.Id);
                    addr.StreetAddress = vm.Address.StreetAddress;
                    addr.LastModifiedBy = loggedInUser.FullName;
                    addr.LastModifiedDate = DateTime.Now;
                    _context.Update(addr);

                    // Update Lot entry
                    var lot = _context.Lot.Find(vm.Lot.LotId);
                    lot.LotNumber = vm.Lot.LotNumber;
                    lot.TaxId = vm.Lot.TaxId;
                    lot.LastModifiedBy = loggedInUser.FullName;
                    lot.LastModifiedDate = DateTime.Now;
                    _context.Update(lot);

                    // Check OwnerLot entry for this owner/lot combination, create if necessary
                    var currOwnerLot = await _context.OwnerLot
                        .Where(u => u.LotId == id)
                        .Where(u => u.EndDate == DateTime.MinValue)
                        .Where(u => u.IsPrimary)
                        .FirstOrDefaultAsync();

                    // Nobody owns this lot right now and we have owners
                    if (currOwnerLot == null && vm.OwnerIds != null)
                    {
                        // Create a new entry for every owner
                        foreach (var oid in vm.OwnerIds)
                        {
                            var newOwnerLot = new OwnerLot
                            {
                                OwnerId = vm.OwnerId,
                                LotId = id,
                                IsPrimary = oid == vm.OwnerId,
                                StartDate = DateTime.Now
                            };

                            _context.Add(newOwnerLot);
                        }
                    }
                    // There is at least one existing relationship
                    else if (currOwnerLot != null && vm.OwnerIds != null)
                    {
                        var prevOwnerLots = await _context.OwnerLot
                            .Where(u => u.LotId == id)
                            .Where(u => u.EndDate == DateTime.MinValue)
                            .ToListAsync();

                        // Check previous OwnerLot entities to see if any were removed
                        foreach (var ol in prevOwnerLots)
                        {
                            // User was removed from the user list, end the relationship
                            if (!vm.OwnerIds.Contains(ol.OwnerId))
                            {
                                ol.EndDate = DateTime.Now;
                            }
                            // The user already has a relationship - don't do anything in the next step
                            else
                            {
                                // Make sure that the primary user is set correctly
                                ol.IsPrimary = ol.OwnerId == vm.OwnerId;
                                vm.OwnerIds.Remove(ol.OwnerId);
                            }
                        }

                        // Create new relationships if necessary
                        foreach (var oid in vm.OwnerIds)
                        {
                            var newOwnerLot = new OwnerLot
                            {
                                OwnerId = oid,
                                LotId = id,
                                IsPrimary = oid == vm.OwnerId,
                                StartDate = DateTime.Now
                            };

                            _context.Add(newOwnerLot);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LotExists(vm.Lot.LotId))
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
            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "Id", lot.AddressId);
            //ViewData["Owner"] = new SelectList(_context.Owner, "OwnerId", "FullName");
            ViewData["OwnerList"] = _context.Owner.ToList();
            return View(vm);
        }

        // GET: Admin/Lots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lot
                .Include(l => l.Address)
                .FirstOrDefaultAsync(m => m.LotId == id);
            if (lot == null)
            {
                return NotFound();
            }

            return View(lot);
        }

        // POST: Admin/Lots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            var lot = await _context.Lot.Include(u => u.Address).FirstOrDefaultAsync(u => u.LotId == id);
            lot.LastModifiedBy = loggedInUser.FullName;
            lot.LastModifiedDate = DateTime.Now;
            lot.IsArchive = true;
            lot.Address.LastModifiedBy = loggedInUser.FullName;
            lot.Address.LastModifiedDate = DateTime.Now;
            lot.Address.IsArchive = true;

            //_context.Lot.Remove(lot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LotExists(int id)
        {
            return _context.Lot.Any(e => e.LotId == id);
        }

        private async Task<List<OwnerLot>> GetLotOwners(int id)
        {
            return await _context.OwnerLot
                .Include(u => u.Owner)
                .Where(u => u.LotId == id)
                .Where(u => u.EndDate == DateTime.MinValue)
                .ToListAsync();
        }

        private async Task<SunridgeHOA.Models.Owner> GetPrimaryOwnerAsync(int id)
        {
            return await _context.OwnerLot
                .Include(u => u.Owner)
                .Where(u => u.LotId == id)
                .Where(u => u.EndDate == DateTime.MinValue)
                .Where(u => u.IsPrimary)
                .Select(u => u.Owner)
                .FirstOrDefaultAsync();
        }

        private async Task<List<SunridgeHOA.Models.Owner>> GetNonPrimaryOwnerAsync(int id)
        {
            return await _context.OwnerLot
                .Include(u => u.Owner)
                .Where(u => u.LotId == id)
                .Where(u => u.EndDate == DateTime.MinValue)
                .Where(u => !u.IsPrimary)
                .Select(u => u.Owner)
                .ToListAsync();
        }
    }
}
