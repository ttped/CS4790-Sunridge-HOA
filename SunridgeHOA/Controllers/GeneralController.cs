using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;
using SunridgeHOA.Models.ViewModels;

namespace SunridgeHOA.Controllers
{
    [Authorize(Roles = "Owner")]
    public class GeneralController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        //[BindProperty]
        //public DashboardViewModel dashboardViewModel { get; set; }

        public GeneralController(ApplicationDbContext context, HostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            
        }

        public async Task<IActionResult> Dashboard()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            // Redirect to the password change page if the user is still using the default password
            var defaultPassword = Areas.Admin.Data.OwnerUtility.GenerateDefaultPassword(loggedInUser);
            if (_userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, defaultPassword) == PasswordVerificationResult.Success)
            {
                return RedirectToPage("/Account/Manage/ChangePassword", new { area = "Identity" });
            }

            var myLots = await _context.OwnerLot
                .Include(u => u.Lot)
                .Where(u => u.OwnerId == loggedInUser.OwnerId)
                .Where(u => !u.IsArchive)
                .Select(u => u.Lot.LotId)
                .ToListAsync();

            var dashboardViewModel = new DashboardViewModel()
            {
                Owner = await _context.Owner.Where(x => x.OwnerId == loggedInUser.OwnerId).FirstAsync(),
                OwnerLots = await _context.OwnerLot.Where(x => x.OwnerId == loggedInUser.OwnerId).ToListAsync(),
                KeyHistories = await _context.KeyHistory.Where(x => myLots.Contains(x.LotId)).ToListAsync()
            };
            dashboardViewModel.Owner.Address = _context.Address.Where(x => x.Id == dashboardViewModel.Owner.AddressId).First();

            foreach (var ownerLot in dashboardViewModel.OwnerLots)
            {
                ownerLot.Lot = await _context.Lot
                    .Include(u => u.LotInventories).ThenInclude(u => u.Inventory)
                    .Where(x => x.LotId == ownerLot.LotId).FirstAsync();
                //foreach (var inventory in ownerLot.Lot.LotInventories)
                //{
                //    inventory.Description = _context.Inventory.Where(x => x.InventoryId == inventory.InventoryId).First().Description;
                //}
                
                foreach(var key in dashboardViewModel.KeyHistories)
                {
                    key.Key = _context.Key.Where(x => x.KeyId == key.KeyId).ToList().First();
                }
                dashboardViewModel.lotInventories = await _context.LotInventory.Where(x => x.LotId == ownerLot.LotId).ToListAsync();
            }

            return View(dashboardViewModel);
        }

        
    }
}