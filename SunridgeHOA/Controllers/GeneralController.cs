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
            var dashboardViewModel = new DashboardViewModel()
            {
                Owner = await _context.Owner.Where(x => x.OwnerId == loggedInUser.OwnerId).FirstAsync(),
                OwnerLots = await _context.OwnerLot.Where(x => x.OwnerId == loggedInUser.OwnerId).ToListAsync(),
                KeyHistories = await _context.KeyHistory.Where(x => x.OwnerId == loggedInUser.OwnerId).ToListAsync()
            };
            dashboardViewModel.Owner.Address = _context.Address.Where(x => x.Id == dashboardViewModel.Owner.AddressId).First();

            foreach (var ownerLot in dashboardViewModel.OwnerLots)
            {
                ownerLot.Lot = await _context.Lot.Where(x => x.LotId == ownerLot.LotId).FirstAsync();
                ownerLot.Lot.LotInventories = await _context.LotInventory
                    .Where(x => x.LotId == 1)
                    .ToListAsync();
                foreach (var inventory in ownerLot.Lot.LotInventories)
                {
                    inventory.Description = _context.Inventory.Where(x => x.InventoryId == inventory.InventoryId).First().Description;
                }
                
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