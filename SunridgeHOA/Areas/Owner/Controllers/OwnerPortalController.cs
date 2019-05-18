using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Areas.Owner.Models.ViewModels;
using SunridgeHOA.Models;

namespace SunridgeHOA.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class OwnerPortalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OwnerPortalController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Edit Limited (Personal) Owner Information
        public async Task<IActionResult> OwnerInfo()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var owner = await _context.Owner
                .Include(u => u.Address)
                .SingleOrDefaultAsync(u => u.OwnerId == user.OwnerId);

            return View(new OwnerInfoVM
            {
                Owner = owner,
                Address = owner.Address
            });

        }

        [HttpPost]
        public async Task<IActionResult> OwnerInfo(OwnerInfoVM vm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user.OwnerId != vm.Owner.OwnerId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var owner = await _context.Owner.SingleOrDefaultAsync(u => u.OwnerId == vm.Owner.OwnerId);
            owner.FirstName = vm.Owner.FirstName;
            owner.LastName = vm.Owner.LastName;
            owner.Occupation = vm.Owner.Occupation;
            owner.Birthday = vm.Owner.Birthday;
            owner.Email = vm.Owner.Email;
            owner.Phone = vm.Owner.Phone;
            owner.EmergencyContactName = vm.Owner.EmergencyContactName;
            owner.EmergencyContactPhone = vm.Owner.EmergencyContactPhone;
            owner.ReceiveEmails = vm.Owner.ReceiveEmails;
            owner.LastModifiedBy = vm.Owner.FullName;
            owner.LastModifiedDate = DateTime.Now;

            var addr = await _context.Address.SingleOrDefaultAsync(u => u.Id == vm.Address.Id);
            addr.StreetAddress = vm.Address.StreetAddress;
            addr.City = vm.Address.City;
            addr.State = vm.Address.State;
            addr.Zip = vm.Address.Zip;
            addr.LastModifiedBy = owner.FullName;
            addr.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard", "General", new { area = "" });
        }
    }
}