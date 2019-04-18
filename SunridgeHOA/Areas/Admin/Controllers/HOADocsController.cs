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

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner")]
    public class HOADocsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HostingEnvironment _hostingEnv;

        public HOADocsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, HostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnv = env;
        }

        public async Task<IActionResult> Index(string query)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");

            var hoa = _context.Lot.SingleOrDefault(u => u.LotNumber == "HOA");

            var filesQuery = _context.LotHistory
                .Include(u => u.Files)
                .Include(u => u.HistoryType)
                .Where(u => u.LotId == hoa.LotId);
            if (!isAdmin)
            {
                filesQuery = filesQuery.Where(u => u.PrivacyLevel != "Admin");
            }

            var files = await filesQuery.ToListAsync();

            ViewData["LotId"] = hoa.LotId;
            return View(files);
        }
    }
}