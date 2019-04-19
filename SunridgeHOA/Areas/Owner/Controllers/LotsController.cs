using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Areas.Owner.Models;
using SunridgeHOA.Areas.Owner.Models.ViewModels;
using SunridgeHOA.Models;
using SunridgeHOA.Utility;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class LotsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HostingEnvironment _hostingEnv;

        public LotsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, HostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnv = env;
        }

        // GET: Admin/Lots
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index(string query)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);

            // Redirect to /MyLots if the user is not an admin
            if (!roles.Contains("Admin") || !roles.Contains("SuperAdmin"))
            {
                return RedirectToAction(nameof(MyLots));
            }

            List<Lot> lots = null;
            // Need to filter the search
            if (!String.IsNullOrEmpty(query))
            {
                lots = await _context.Lot
                    .Include(l => l.Address)
                    .Where(u => u.LotNumber.ToLower().Contains(query.ToLower())) // use contains so searching "H2" picks up all H2** lots
                    .Where(u => u.LotNumber != "HOA") // exclude generic HOA lot
                    .OrderBy(u => u.LotNumber)
                    .ToListAsync();
            }
            // No search string - include all lots
            else
            {
                lots = await _context.Lot
                       .Include(l => l.Address)
                       .Where(u => u.LotNumber != "HOA")
                       .OrderBy(u => u.LotNumber)
                       .ToListAsync();
            }

            var vm = new List<LotIndexVM>();

            foreach (var lot in lots)
            {
                var owners = _context.OwnerLot
                    .Include(u => u.Owner)
                    .Where(u => u.LotId == lot.LotId)
                    .Where(u => !u.IsArchive);

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
                        Owners = new List<SunridgeHOA.Models.Owner>(),
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

        public async Task<IActionResult> MyLots()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            var myLots = await _context.OwnerLot
                .Include(u => u.Lot)
                .Where(u => u.OwnerId == loggedInUser.OwnerId)
                .Where(u => !u.IsArchive)
                .Select(u => u.Lot)
                .ToListAsync();

            var lotList = new List<LotIndexVM>();
            foreach (var lot in myLots)
            {
                var owners = _context.OwnerLot
                    .Include(u => u.Owner)
                    .Where(u => u.LotId == lot.LotId)
                    .Where(u => !u.IsArchive);

                var lotItems = await _context.LotInventory
                    .Include(u => u.Inventory)
                    .Where(u => u.LotId == lot.LotId)
                    .Select(u => u.Inventory)
                    .ToListAsync();

                var lotVM = new LotIndexVM
                {
                    Lot = lot,
                    Address = _context.Address.Find(lot.AddressId),
                    PrimaryOwner = owners.Where(u => u.IsPrimary).First().Owner,
                    Owners = owners.Where(u => !u.IsPrimary).Select(u => u.Owner).ToList(),
                    InventoryItems = lotItems
                };

                lotList.Add(lotVM);
            }

            return View(lotList);
        }

        public async Task<IActionResult> EditLotItems(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lot
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.LotId == id);
            if (lot == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            var isOwner = _context.OwnerLot
                .Where(u => u.LotId == id)
                .Where(u => u.OwnerId == loggedInUser.OwnerId)
                .Where(u => !u.IsArchive)
                .Any();
            if (!isOwner)
            {
                return NotFound();
            }

            var lotItems = await _context.LotInventory
                .Include(u => u.Inventory)
                .Where(u => u.LotId == id)
                .Select(u => u.Inventory.InventoryId)
                .ToListAsync();

            var vm = new LotEditInventoryVM
            {
                Lot = lot,
                Address = lot.Address,
                SelectedItems = lotItems
            };

            ViewData["Inventory"] = _context.Inventory.ToList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLotItems(int id, LotEditInventoryVM vm)
        {
            if (id != vm.Lot.LotId)
            {
                return NotFound();
            }

            var lot = await _context.Lot
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.LotId == id);
            if (lot == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            var isOwner = _context.OwnerLot
                .Where(u => u.LotId == id)
                .Where(u => u.OwnerId == loggedInUser.OwnerId)
                .Where(u => !u.IsArchive)
                .Any();
            if (!isOwner)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Get the current inventory relationships
                var currItems = _context.LotInventory
                    .Where(u => u.LotId == id)
                    .ToList();


                foreach (var item in currItems)
                {
                    // Item is still selected - remove it for next step
                    if (vm.SelectedItems != null && vm.SelectedItems.Contains(item.InventoryId))
                    {
                        vm.SelectedItems.Remove(item.InventoryId);
                    }
                    // Item is not selected anymore - need to remove the relationship
                    else
                    {
                        _context.LotInventory.Remove(item);
                    }
                }

                // Any items still in SelectedItems need to be added in the relationship table
                if (vm.SelectedItems != null)
                {
                    foreach (var invId in vm.SelectedItems)
                    {
                        _context.LotInventory.Add(new LotInventory
                        {
                            LotId = id,
                            InventoryId = invId,
                            LastModifiedBy = loggedInUser.FullName,
                            LastModifiedDate = DateTime.Now
                        });
                    }
                }
                

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyLots));
            }

            ViewData["Inventory"] = _context.Inventory.ToList();
            return View(null);
        }

        public async Task<IActionResult> AddDocument(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = _context.Lot.Find(id);
            if (lot == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");

            if (lot.LotNumber == "HOA" && !isAdmin)
            {
                return NotFound();
            }

            //var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            var ownerLots = _context.OwnerLot
                .Where(u => u.LotId == id)
                .Where(u => u.OwnerId == identityUser.OwnerId);
            if (!isAdmin && !ownerLots.Any())
            {
                return NotFound();
            }

            ViewData["LotId"] = lot.LotId;
            ViewData["HistoryTypes"] = new SelectList(_context.HistoryType, "HistoryTypeId", "Description");

            return View(new DocumentVM
            {
                Id = lot.LotId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDocument(int id, DocumentVM vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            var lot = _context.Lot.Find(vm.Id);

            var files = HttpContext.Request.Form.Files;
            if (files.Count == 0)
            {
                ModelState.AddModelError("Files", "Please upload at least one file");
            }

            if (ModelState.IsValid)
            {
                var uploadFiles = new List<SunridgeHOA.Models.File>();
                foreach (var file in files)
                {
                    var webRootPath = _hostingEnv.WebRootPath;
                    var folder = SD.LotDocsFolder;
                    var uploads = Path.Combine(webRootPath, folder);
                    var name = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    var dateExt = DateTime.Now.ToString("MMddyyyy");
                    var newFileName = $"{lot.LotNumber} - {name} {dateExt}{extension}";

                    using (var filestream = new FileStream(Path.Combine(uploads, newFileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    var uploadFile = new SunridgeHOA.Models.File
                    {
                        FileURL = $@"\{folder}\{newFileName}",
                        Date = DateTime.Now,
                        Description = Path.GetFileName(file.FileName)
                    };
                    _context.File.Add(uploadFile);
                    uploadFiles.Add(uploadFile);
                    //await _context.SaveChangesAsync();
                }

                var lotHistory = new LotHistory
                {
                    LotId = lot.LotId,
                    HistoryTypeId = vm.HistoryType,
                    Date = DateTime.Now,
                    Description = vm.Description,
                    PrivacyLevel = vm.AdminOnly ? "Admin" : "Owner",
                    LastModifiedBy = loggedInUser.FullName,
                    LastModifiedDate = DateTime.Now,
                    Files = uploadFiles
                };
                _context.LotHistory.Add(lotHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewFiles), new { id = id });
            }

            ViewData["LotId"] = lot.LotId;
            ViewData["HistoryTypes"] = new SelectList(_context.HistoryType, "HistoryTypeId", "Description", vm.HistoryType);
            return View(vm);
        }

        public async Task<IActionResult> ViewFiles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lot.SingleOrDefaultAsync(u => u.LotId == id);
            if (lot == null)
            {
                return NotFound();
            }
            else if (lot.LotNumber == "HOA")
            {
                return RedirectToAction("Index", "HOADocs", new { area = "Admin" });
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");
            //var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            var ownerLots = _context.OwnerLot
                .Where(u => u.LotId == id)
                .Where(u => u.OwnerId == identityUser.OwnerId);
            if (!isAdmin && !ownerLots.Any())
            {
                return NotFound();
            }

            var filesQuery = _context.LotHistory
                .Include(u => u.Files)
                .Include(u => u.HistoryType)
                .Where(u => u.LotId == id);
            if (!isAdmin)
            {
                filesQuery = filesQuery.Where(u => u.PrivacyLevel != "Admin");
            }

            var files = await filesQuery.ToListAsync();

            ViewData["LotId"] = lot.LotId;
            ViewData["LotNumber"] = lot.LotNumber;
            return View(files);
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

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");
            //var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            var ownerLots = _context.OwnerLot
                .Where(u => u.LotId == id)
                .Where(u => u.OwnerId == identityUser.OwnerId);
            if (!isAdmin && !ownerLots.Any())
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
        [Authorize(Roles = "SuperAdmin, Admin")]
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
        [Authorize(Roles = "SuperAdmin, Admin")]
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
        [Authorize(Roles = "SuperAdmin, Admin")]
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
                .Where(u => !u.IsArchive)
                .Select(u => u.Owner.OwnerId)
                .ToListAsync();

            var lotItems = await _context.LotInventory
                .Include(u => u.Inventory)
                .Where(u => u.LotId == id)
                .Select(u => u.Inventory.InventoryId)
                .ToListAsync();

            var vm = new LotEditVM
            {
                Lot = lot,
                Address = lot.Address,
                OwnerId = currPrimary?.OwnerId ?? -1,
                OwnerIds = currOwnerIds,
                SelectedItems = lotItems
            };

            ViewData["OwnerList"] = _context.Owner.ToList();
            ViewData["Inventory"] = _context.Inventory.ToList();
            return View(vm);
        }

        // POST: Admin/Lots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Edit(int id, LotEditVM vm)
        {
            if (id != vm.Lot.LotId)
            {
                return NotFound();
            }

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
                        .Where(u => !u.IsArchive)
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
                            .Where(u => !u.IsArchive)
                            .ToListAsync();

                        // Check previous OwnerLot entities to see if any were removed
                        foreach (var ol in prevOwnerLots)
                        {
                            // User was removed from the user list, end the relationship
                            if (!vm.OwnerIds.Contains(ol.OwnerId))
                            {
                                ol.EndDate = DateTime.Now;
                                ol.IsArchive = true;
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

                    // Need to check the inventory relationships
                    var currItems = _context.LotInventory
                       .Where(u => u.LotId == id)
                       .ToList();

                    foreach (var item in currItems)
                    {
                        // Item is still selected - remove it for next step
                        if (vm.SelectedItems.Contains(item.InventoryId))
                        {
                            vm.SelectedItems.Remove(item.InventoryId);
                        }
                        // Item is not selected anymore - need to remove the relationship
                        else
                        {
                            _context.LotInventory.Remove(item);
                        }
                    }

                    // Any items still in SelectedItems need to be added in the relationship table
                    if (vm.SelectedItems != null)
                    {
                        foreach (var invId in vm.SelectedItems)
                        {
                            _context.LotInventory.Add(new LotInventory
                            {
                                LotId = id,
                                InventoryId = invId,
                                LastModifiedBy = loggedInUser.FullName,
                                LastModifiedDate = DateTime.Now
                            });
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
        [Authorize(Roles = "SuperAdmin, Admin")]
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
        [Authorize(Roles = "SuperAdmin, Admin")]
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
                .Where(u => !u.IsArchive)
                .ToListAsync();
        }

        private async Task<SunridgeHOA.Models.Owner> GetPrimaryOwnerAsync(int id)
        {
            return await _context.OwnerLot
                .Include(u => u.Owner)
                .Where(u => u.LotId == id)
                .Where(u => !u.IsArchive)
                .Where(u => u.IsPrimary)
                .Select(u => u.Owner)
                .FirstOrDefaultAsync();
        }

        private async Task<List<SunridgeHOA.Models.Owner>> GetNonPrimaryOwnerAsync(int id)
        {
            return await _context.OwnerLot
                .Include(u => u.Owner)
                .Where(u => u.LotId == id)
                .Where(u => !u.IsArchive)
                .Where(u => !u.IsPrimary)
                .Select(u => u.Owner)
                .ToListAsync();
        }
    }
}
