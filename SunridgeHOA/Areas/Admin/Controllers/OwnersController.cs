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
using SunridgeHOA.Areas.Admin.Models;
using SunridgeHOA.Areas.Owner.Models.ViewModels;
using SunridgeHOA.Models;
using SunridgeHOA.Utility;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class OwnersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HostingEnvironment _hostingEnv;

        public OwnersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, HostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnv = env;
        }

        // GET: Admin/Owners
        public async Task<IActionResult> Index(string query)
        {
            List<SunridgeHOA.Models.Owner> owners = null;

            // Need to filter the search
            if (!String.IsNullOrEmpty(query))
            {
                owners = await _context.Owner
                    .Include(u => u.Address)
                    .Where(u => u.FullName.ToLower().Contains(query.ToLower()))
                    .ToListAsync();
            }
            // No search - include all owners
            else
            {
                owners = await _context.Owner
                    .Include(u => u.Address)
                    .ToListAsync();
            }

            var vmList = new List<OwnerIndexVM>();
            foreach (var owner in owners)
            {
                var lots = await _context.OwnerLot
                    .Include(u => u.Lot)
                    .Where(u => u.OwnerId == owner.OwnerId)
                    .Where(u => !u.IsArchive)
                    .Select(u => u.Lot.LotNumber)
                    .ToListAsync();

                vmList.Add(new OwnerIndexVM
                {
                    Owner = owner,
                    Lots = lots
                });
            }

            return View(vmList);
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
                .FirstOrDefaultAsync(m => m.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.FindByIdAsync(owner.ApplicationUserId);
            var roles = await _userManager.GetRolesAsync(identityUser);
            ViewData["IsAdmin"] = roles.Contains("Admin") || roles.Contains("SuperAdmin");
            return View(owner);
        }

        public async Task<IActionResult> AddDocument(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = _context.Owner.Find(id);
            if (owner == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");
            if (!isAdmin && id != identityUser.OwnerId)
            {
                return NotFound();
            }

            ViewData["OwnerId"] = owner.OwnerId;
            ViewData["HistoryTypes"] = new SelectList(_context.HistoryType, "HistoryTypeId", "Description");

            return View(new DocumentVM
            {
                Id = owner.OwnerId
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

            var owner = _context.Owner.Find(vm.Id);

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
                    var uploads = Path.Combine(webRootPath, SD.OwnerDocsFolder);
                    var name = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    var dateExt = DateTime.Now.ToString("MMddyyyy");
                    var newFileName = $"{owner.OwnerId} - {name} {dateExt}{extension}";

                    using (var filestream = new FileStream(Path.Combine(uploads, newFileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    var uploadFile = new SunridgeHOA.Models.File
                    {
                        FileURL = $@"\{SD.OwnerDocsFolder}\{newFileName}",
                        Date = DateTime.Now,
                        Description = Path.GetFileName(file.FileName)
                    };
                    _context.File.Add(uploadFile);
                    uploadFiles.Add(uploadFile);
                    //await _context.SaveChangesAsync();
                }

                var ownerHistory = new OwnerHistory
                {
                    OwnerId = owner.OwnerId,
                    HistoryTypeId = vm.HistoryType,
                    Date = DateTime.Now,
                    Description = vm.Description,
                    LastModifiedBy = loggedInUser.FullName,
                    LastModifiedDate = DateTime.Now,
                    Files = uploadFiles
                };
                _context.OwnerHistory.Add(ownerHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewFiles), new { id = id });
            }

            ViewData["OwnerId"] = owner.OwnerId;
            ViewData["HistoryTypes"] = new SelectList(_context.HistoryType, "HistoryTypeId", "Description", vm.HistoryType);
            return View(vm);
        }

        public async Task<IActionResult> ViewFiles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owner.SingleOrDefaultAsync(u => u.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");
            //var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            if (!isAdmin && identityUser.OwnerId != id)
            {
                return NotFound();
            }

            var files = await _context.OwnerHistory
                .Include(u => u.Files)
                .Include(u => u.HistoryType)
                .Where(u => u.OwnerId == id)
                .ToListAsync();

            ViewData["OwnerId"] = owner.OwnerId;
            ViewData["FullName"] = owner.FullName;
            return View(files);
        }

        public async Task<IActionResult> LoginInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owner.FirstOrDefaultAsync(u => u.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(owner.ApplicationUserId);

            ViewData["FullName"] = owner.FullName;

            return View(new UserInfoVM
            {
                Username = user.UserName,
                UserId = user.Id
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoginInfo(int? id, UserInfoVM vm)
        {
            var owner = await _context.Owner.FindAsync(id);
            var user = await _userManager.FindByIdAsync(owner.ApplicationUserId);
            if (owner == null || user == null)
            {
                return NotFound();
            }

            // Need to see if we are changing the username, and if the username exists already
            var existingUser = await _userManager.FindByNameAsync(vm.Username);
            if (vm.Username.ToLower() != user.UserName.ToLower() && existingUser != null)
            {
                ModelState.AddModelError("Username", "There is already a user with that username");
                return View(new UserInfoVM
                {
                    Username = vm.Username,
                    UserId = user.Id
                });
            }

            // Set the username and password
            user.UserName = vm.Username;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, vm.Password);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return View(new UserInfoVM
                {
                    Username = vm.Username,
                    UserId = user.Id
                });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Owners/Create
        public IActionResult Create()
        {
            ViewData["LotsSelect"] = new SelectList(_context.Lot.OrderBy(u => u.LotNumber).ToList(), "LotId", "LotNumber");
            return View();
        }

        // POST: Admin/Owners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OwnerVM vm)
        {
            if (vm.Owner.Email != null)
            {
                var existingUser = await _userManager.FindByEmailAsync(vm.Owner.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "There is an existing user with that email address");
                }
            }
            
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                vm.Address.LastModifiedBy = loggedInUser.FullName;
                vm.Address.LastModifiedDate = DateTime.Now;
                _context.Add(vm.Address);

                vm.Owner.Address = vm.Address;
                vm.Owner.LastModifiedBy = loggedInUser.FullName;
                vm.Owner.LastModifiedDate = DateTime.Now;

                // Don't save yet - need to link the Owner to the ApplicationUser
                _context.Add(vm.Owner);
                await _context.SaveChangesAsync();

                // Find a default username - adds a number to the end if there is a duplicate
                var username = $"{vm.Owner.FirstName}{vm.Owner.LastName}";
                int count = 0;
                while (await _userManager.FindByNameAsync(username) != null)
                {
                    count++;
                    username = $"{username}{count}";
                }

                // Create user with default credentials
                //  - Username: FirstnameLastname (e.g. JessBrunker)
                //  - Password: SunridgeUsername123$ (e.g. SunridgeJessBrunker123$)
                var newOwner = new ApplicationUser
                {
                    UserName = username,
                    Email = vm.Owner.Email,
                    OwnerId = vm.Owner.OwnerId
                };

                var defaultPassword = $"Sunridge{username}123$";
                var result = await _userManager.CreateAsync(newOwner, defaultPassword);
                if (result.Succeeded)
                {
                    var roles = new List<string> { "Owner" };
                    if (vm.IsAdmin)
                    {
                        roles.Add("Admin");
                    };
                    await _userManager.AddToRolesAsync(newOwner, roles);

                    // Link Owner to the Application User
                    vm.Owner.ApplicationUserId = newOwner.Id;
                    //_context.Add(vm.Owner);
                    await _context.SaveChangesAsync();

                    // Add the Owner to a Lot
                    if (vm.LotId != 0)
                    {
                        _context.OwnerLot.Add(new OwnerLot
                        {
                            LotId = vm.LotId,
                            OwnerId = vm.Owner.OwnerId,
                            StartDate = DateTime.Now
                        });

                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["LotsSelect"] = new SelectList(_context.Lot.OrderBy(u => u.LotNumber).ToList(), "LotId", "LotNumber", vm.LotId);
            return View(vm);
        }

        public IActionResult CreateMany()
        {
            ViewData["LotsSelect"] = new SelectList(_context.Lot.OrderBy(u => u.LotNumber).ToList(), "LotId", "LotNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMany(BatchOwnerCreateVM vm)
        {
            var validUsers = new List<MinOwnerInfo>();
            foreach (var owner in vm.OwnerList)
            {
                if ((String.IsNullOrEmpty(owner.FirstName) && !String.IsNullOrEmpty(owner.LastName)) ||
                    (!String.IsNullOrEmpty(owner.FirstName) && String.IsNullOrEmpty(owner.LastName)))
                {
                    ModelState.AddModelError("OwnerList", "Make sure all first names have a matching last name");
                }

                if (!String.IsNullOrEmpty(owner.FirstName) && !String.IsNullOrEmpty(owner.LastName))
                {
                    validUsers.Add(owner);
                }
            }

            if (!validUsers.Any())
            {
                ModelState.AddModelError("OwnerList", "Please enter at least one user");
            }

            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                vm.Address.LastModifiedBy = loggedInUser.FullName;
                vm.Address.LastModifiedDate = DateTime.Now;
                _context.Add(vm.Address);

                foreach (var ownerInfo in validUsers)
                {
                    var owner = new SunridgeHOA.Models.Owner
                    {
                        Address = vm.Address,
                        FirstName = ownerInfo.FirstName,
                        LastName = ownerInfo.LastName,
                        Email = ownerInfo.Email,
                        LastModifiedBy = loggedInUser.FullName,
                        LastModifiedDate = DateTime.Now
                    };

                    _context.Add(owner);
                    await _context.SaveChangesAsync();

                    // Find a default username - adds a number to the end if there is a duplicate
                    var username = $"{owner.FirstName}{owner.LastName}";
                    int count = 0;
                    while (await _userManager.FindByNameAsync(username) != null)
                    {
                        count++;
                        username = $"{username}{count}";
                    }

                    // Create user with default credentials
                    //  - Username: FirstnameLastname (e.g. JessBrunker)
                    //  - Password: SunridgeUsername123$ (e.g. SunridgeJessBrunker123$)
                    var newOwner = new ApplicationUser
                    {
                        UserName = username,
                        Email = owner.Email,
                        OwnerId = owner.OwnerId
                    };

                    var defaultPassword = $"Sunridge{username}123$";
                    var result = await _userManager.CreateAsync(newOwner, defaultPassword);
                    if (result.Succeeded)
                    {
                        var roles = new List<string> { "Owner" };
                        if (ownerInfo.IsAdmin)
                        {
                            roles.Add("Admin");
                        };
                        await _userManager.AddToRolesAsync(newOwner, roles);

                        // Link Owner to the Application User
                        owner.ApplicationUserId = newOwner.Id;
                        //_context.Add(vm.Owner);
                        await _context.SaveChangesAsync();

                        // Add the Owner to a Lot
                        if (vm.LotId != -1)
                        {
                            _context.OwnerLot.Add(new OwnerLot
                            {
                                LotId = vm.LotId,
                                OwnerId = owner.OwnerId,
                                StartDate = DateTime.Now
                            });

                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["LotsSelect"] = new SelectList(_context.Lot.OrderBy(u => u.LotNumber).ToList(), "LotId", "LotNumber", vm.LotId);
            return View();
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

            var appUser = await _userManager.FindByIdAsync(owner.ApplicationUserId);
            var roles = await _userManager.GetRolesAsync(appUser);

            var vm = new OwnerVM
            {
                Owner = owner,
                Address = owner.Address,
                IsAdmin = roles.Contains("Admin")
            };

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["CurrUserId"] = identityUser.Id;

            return View(vm);
        }

        // POST: Admin/Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OwnerVM vm)
        {
            if (id != vm.Owner.OwnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                    var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

                    var addr = await _context.Address.SingleOrDefaultAsync(u => u.Id == vm.Address.Id);
                    addr.StreetAddress = vm.Address.StreetAddress;
                    addr.City = vm.Address.City;
                    addr.State = vm.Address.State;
                    addr.Zip = vm.Address.Zip;
                    addr.LastModifiedBy = loggedInUser.FullName;
                    addr.LastModifiedDate = DateTime.Now;
                    _context.Update(addr);

                    var owner = await _context.Owner.SingleOrDefaultAsync(u => u.OwnerId == vm.Owner.OwnerId);
                    owner.FirstName = vm.Owner.FirstName;
                    owner.LastName = vm.Owner.LastName;
                    owner.Occupation = vm.Owner.Occupation;
                    owner.Birthday = vm.Owner.Birthday;
                    owner.Email = vm.Owner.Email;
                    owner.Phone = vm.Owner.Phone;
                    owner.EmergencyContactName = vm.Owner.EmergencyContactName;
                    owner.EmergencyContactPhone = vm.Owner.EmergencyContactPhone;
                    //vm.Owner.AddressId = addr.Id; // need to reset this or the database gets mad
                    //vm.Owner.LastModifiedBy = loggedInUser.FullName;
                    //vm.Owner.LastModifiedDate = DateTime.Now;
                    _context.Update(owner);

                    await _context.SaveChangesAsync();

                    var appUser = await _userManager.FindByIdAsync(owner.ApplicationUserId);
                    var roles = await _userManager.GetRolesAsync(appUser);
                    if (vm.IsAdmin)
                    {
                        await _userManager.AddToRoleAsync(appUser, "Admin");
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(appUser, "Admin");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(vm.Owner.OwnerId))
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

            return View(vm);
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
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id)
        {
            return _context.Owner.Any(e => e.OwnerId == id);
        }
    }
}
