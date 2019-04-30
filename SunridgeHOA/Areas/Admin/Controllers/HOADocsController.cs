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
using SunridgeHOA.Areas.Owner.Models.ViewModels;
using SunridgeHOA.Models;
using SunridgeHOA.Utility;

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

        //public async Task<IActionResult> Index(string query)
        //{
        //    var identityUser = await _userManager.GetUserAsync(HttpContext.User);
        //    var roles = await _userManager.GetRolesAsync(identityUser);
        //    var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");

        //    var hoa = _context.Lot.SingleOrDefault(u => u.LotNumber == "HOA");

        //    var filesQuery = _context.LotHistory
        //        .Include(u => u.Files)
        //        .Include(u => u.HistoryType)
        //        .Where(u => u.LotId == hoa.LotId);
        //    if (!isAdmin)
        //    {
        //        filesQuery = filesQuery.Where(u => u.PrivacyLevel != "Admin");
        //    }

        //    var files = await filesQuery.ToListAsync();

        //    ViewData["LotId"] = hoa.LotId;
        //    return View(files);
        //}

        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddDocument()
        //{
        //    var lot = _context.Lot.Single(u => u.LotNumber == "HOA");
        //    //if (lot == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    var identityUser = await _userManager.GetUserAsync(HttpContext.User);
        //    var roles = await _userManager.GetRolesAsync(identityUser);
        //    var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");

        //    if (!isAdmin)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["HistoryTypes"] = new SelectList(_context.HistoryType, "HistoryTypeId", "Description");

        //    return View(new DocumentVM
        //    {
        //        Id = lot.LotId
        //    });
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddDocument(DocumentVM vm)
        //{
        //    var identityUser = await _userManager.GetUserAsync(HttpContext.User);
        //    var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

        //    var lot = _context.Lot.Single(u => u.LotNumber == "HOA");

        //    var files = HttpContext.Request.Form.Files;
        //    if (files.Count == 0)
        //    {
        //        ModelState.AddModelError("Files", "Please upload at least one file");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var uploadFiles = new List<SunridgeHOA.Models.File>();
        //        foreach (var file in files)
        //        {
        //            var webRootPath = _hostingEnv.WebRootPath;
        //            var folder = SD.LotDocsFolder;
        //            var uploads = Path.Combine(webRootPath, folder);
        //            var name = Path.GetFileNameWithoutExtension(file.FileName);
        //            var extension = Path.GetExtension(file.FileName);
        //            var dateExt = DateTime.Now.ToString("MMddyyyy");
        //            var newFileName = $"{lot.LotNumber} - {name} {dateExt}{extension}";

        //            using (var filestream = new FileStream(Path.Combine(uploads, newFileName), FileMode.Create))
        //            {
        //                file.CopyTo(filestream);
        //            }

        //            var uploadFile = new SunridgeHOA.Models.File
        //            {
        //                FileURL = $@"\{folder}\{newFileName}",
        //                Date = DateTime.Now,
        //                Description = Path.GetFileName(file.FileName)
        //            };
        //            _context.File.Add(uploadFile);
        //            uploadFiles.Add(uploadFile);
        //            //await _context.SaveChangesAsync();
        //        }

        //        var lotHistory = new LotHistory
        //        {
        //            LotId = lot.LotId,
        //            HistoryTypeId = vm.HistoryType,
        //            Date = DateTime.Now,
        //            Description = vm.Description,
        //            PrivacyLevel = vm.AdminOnly ? "Admin" : "Owner",
        //            LastModifiedBy = loggedInUser.FullName,
        //            LastModifiedDate = DateTime.Now,
        //            Files = uploadFiles
        //        };
        //        _context.LotHistory.Add(lotHistory);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewData["HistoryTypes"] = new SelectList(_context.HistoryType, "HistoryTypeId", "Description", vm.HistoryType);
        //    return View(vm);
        //}
    }
}