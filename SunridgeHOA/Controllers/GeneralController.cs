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
            var loggedInUser = await _context.Owner.Include(u => u.Address).SingleOrDefaultAsync(u => u.OwnerId == identityUser.OwnerId);

            // Redirect to the password change page if the user is still using the default password
            var defaultPassword = Areas.Admin.Data.OwnerUtility.GenerateDefaultPassword(loggedInUser);
            if (_userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, defaultPassword) == PasswordVerificationResult.Success)
            {
                return RedirectToPage("/Account/Manage/ChangePassword", new { area = "Identity" });
            }

            var myLots = await _context.OwnerLot
                .Include(u => u.Lot)
                    .ThenInclude(u => u.LotInventories)
                .Where(u => u.OwnerId == loggedInUser.OwnerId)
                .Where(u => !u.IsArchive)
                .Select(u => u.Lot)
                .ToListAsync();

            var myLotIds = myLots.Select(u => u.LotId).ToList();

            var myKeys = await _context.KeyHistory
                .Include(u => u.Key)
                .Where(u => myLotIds.Contains(u.LotId))
                .ToListAsync();

            var dashboardViewModel = new DashboardViewModel()
            {
                Owner = loggedInUser,
                Lots = myLots,
                KeyHistories = myKeys
            };

            return View(dashboardViewModel);
        }
    }
}