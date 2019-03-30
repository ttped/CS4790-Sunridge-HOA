using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Areas.Admin.Models;
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
            var applicationDbContext = _context.Owner.Include(o => o.Address);

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
            return View();
        }

        // POST: Admin/Owners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OwnerVM vm)
        {
            var existingUser = await _userManager.FindByEmailAsync(vm.Owner.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "There is an existing user with that email address");
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
                _context.Add(vm.Owner);
                await _context.SaveChangesAsync();

                // Find a default username - adds a number to the end if there is a duplicate
                var username = $"{vm.Owner.FirstName}{vm.Owner.LastName}";
                int count = 0;
                while (await _userManager.FindByEmailAsync(username) != null)
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
                }

                return RedirectToAction(nameof(Index));
            }

            return View(vm);

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

            var appUser = await _userManager.FindByEmailAsync(owner.Email);
            var roles = await _userManager.GetRolesAsync(appUser);

            var vm = new OwnerVM
            {
                Owner = owner,
                Address = owner.Address,
                IsAdmin = roles.Contains("Admin")
            };

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

                    var addr = await _context.Address.SingleOrDefaultAsync(u => u.Id == vm.Owner.AddressId);
                    addr.StreetAddress = vm.Address.StreetAddress;
                    addr.City = vm.Address.City;
                    addr.State = vm.Address.State;
                    addr.Zip = vm.Address.Zip;
                    addr.LastModifiedBy = loggedInUser.FullName;
                    addr.LastModifiedDate = DateTime.Now;
                    _context.Update(addr);

                    vm.Owner.LastModifiedBy = loggedInUser.FullName;
                    vm.Owner.LastModifiedDate = DateTime.Now;
                    _context.Update(vm.Owner);

                    await _context.SaveChangesAsync();

                    var appUser = await _userManager.FindByEmailAsync(vm.Owner.Email);
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
