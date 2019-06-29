using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;
using SunridgeHOA.Models.ViewModels;

namespace SunridgeHOA.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class ClassifiedsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassifiedsController(ApplicationDbContext context, HostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            // Need to redirect if user is not in an admin role
            var roles = await _userManager.GetRolesAsync(identityUser);
            if (!roles.Contains("Admin") && !roles.Contains("SuperAdmin"))
            {
                return RedirectToAction(nameof(MyClassifieds));
            }

            var classifieds = await _context.ClassifiedListing
                .Include(u => u.ClassifiedCategory)
                .Include(u => u.Owner)
                .Where(u => !u.IsArchive)
                .ToListAsync();

            return View(classifieds);
        }

        public async Task<ActionResult> MyClassifieds()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            var classifieds = await _context.ClassifiedListing
                .Include(u => u.ClassifiedCategory)
                .Include(u => u.Owner)
                .Where(u => !u.IsArchive)
                .Where(u => u.OwnerId == loggedInUser.OwnerId)
                .ToListAsync();

            return View(classifieds);
        }

        // GET: Classifieds/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            var item = await _context.ClassifiedListing
                .Include(u => u.ClassifiedCategory)
                .Include(u => u.Images)
                .SingleOrDefaultAsync(m => m.ClassifiedListingId == id);
            if (item == null)
            {
                return NotFound();
            }

            // Redirect to specific action for service details
            if (item.ClassifiedCategoryId == 3)
            {
                return RedirectToAction(nameof(ServiceDetails), new { id });
            }

            return View(item);
        }

        

        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_context.ClassifiedCategory.Where(u => u.Description != "Other"), "ClassifiedCategoryId", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassifiedListing listing)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = await _context.Owner.FindAsync(identityUser.OwnerId);

                listing.LastModifiedBy = loggedInUser.FullName;
                listing.LastModifiedDate = DateTime.Now;
                listing.ListingDate = DateTime.Now;
                listing.Owner = loggedInUser;
                listing.OwnerId = loggedInUser.OwnerId;

                _context.ClassifiedListing.Add(listing);
                await _context.SaveChangesAsync();

                //image uploading
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var uploads = Path.Combine(webRootPath, @"img\ClassifiedsImages");

                int i = 0;
                foreach (var file in files)
                {
                    i++;
                    var extension = Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(uploads, listing.ClassifiedListingId + @"_" + i + extension), FileMode.Create))
                    {
                        file.CopyTo(filestream); // moves to server and renames
                    }
                    var image = new ClassifiedImage()
                    {
                        ClassifiedListingId = listing.ClassifiedListingId,
                        IsMainImage = (file == files.First()),
                        ImageExtension = extension,
                        ImageURL = @"\" + @"img\ClassifiedsImages" + @"\" + listing.ClassifiedListingId + @"_" + i + extension
                    };

                    _context.ClassifiedImage.Add(image);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Category"] = new SelectList(_context.ClassifiedCategory.Where(u => u.Description != "Other"), "ClassifiedCategoryId", "Description", listing.ClassifiedCategoryId);
            return View(listing);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.ClassifiedListing
                .Include(u => u.Images)
                .SingleOrDefaultAsync(u => u.ClassifiedListingId == id);
            if (item == null)
            {
                return NotFound();
            }

            // Only admins and the owner can edit the classified
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            var roles = await _userManager.GetRolesAsync(identityUser);
            if (!roles.Contains("Admin") && !roles.Contains("SuperAdmin") && item.OwnerId != loggedInUser.OwnerId)
            {
                return NotFound();
            }

            // Item is a service
            if (item.ClassifiedCategoryId == 3)
            {
                return RedirectToAction(nameof(EditService), new { id });
            }

            ViewData["Category"] = new SelectList(_context.ClassifiedCategory.Where(u => u.Description != "Other"), "ClassifiedCategoryId", "Description", item.ClassifiedCategoryId);
            return View(item);
        }

        // POST: Classifieds/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, ClassifiedListing listing)
        {
            if (id != listing.ClassifiedListingId)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = await _context.Owner.FindAsync(identityUser.OwnerId);

            if (ModelState.IsValid)
            {
                listing.LastModifiedBy = loggedInUser.FullName;
                listing.LastModifiedDate = DateTime.Now;
                listing.OwnerId = loggedInUser.OwnerId;

                _context.Update(listing);
                await _context.SaveChangesAsync();

                var files = HttpContext.Request.Form.Files;
                if (files.Count != 0)
                {
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    
                    var uploads = Path.Combine(webRootPath, @"img\ClassifiedsImages");

                    var oldImages = await _context.ClassifiedImage.Where(x => x.ClassifiedListingId == listing.ClassifiedListingId).ToListAsync();
                    foreach (var oldImage in oldImages)
                    {
                        if (System.IO.File.Exists(Path.Combine(webRootPath, oldImage.ImageURL.Substring(1))))
                        {
                            System.IO.File.Delete(Path.Combine(webRootPath, oldImage.ImageURL.Substring(1)));
                        }
                        _context.ClassifiedImage.Remove(oldImage);
                    }

                    int i = 0;
                    foreach (var file in files)
                    {
                        i++;
                        var extension = Path.GetExtension(file.FileName);
                        using (var filestream = new FileStream(Path.Combine(uploads, listing.ClassifiedListingId + @"_" + i + extension), FileMode.Create))
                        {
                            file.CopyTo(filestream); // moves to server and renames
                        }
                        var image = new ClassifiedImage()
                        {
                            ClassifiedListingId = listing.ClassifiedListingId,
                            IsMainImage = (file == files.First()),
                            ImageExtension = extension,
                            ImageURL = @"\" + @"img\ClassifiedsImages" + @"\" + listing.ClassifiedListingId + @"_" + i + extension
                        };

                        _context.ClassifiedImage.Add(image);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            

            ViewData["Category"] = new SelectList(_context.ClassifiedCategory, "ClassifiedCategoryId", "Description", listing.ClassifiedCategoryId);
            return View(listing);
        }

        public async Task<IActionResult> ServiceDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _context.ClassifiedListing
                .Include(u => u.Images)
                .Include(u => u.Owner)
                .SingleOrDefaultAsync(u => u.ClassifiedListingId == id);

            return View(listing);
        }

        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost, ActionName("AddService")]
        public async Task<IActionResult> AddServicePOST(string description)
        {
            if (String.IsNullOrEmpty(description))
            {
                ModelState.AddModelError("Description", "Please enter a description");
            }

            var files = HttpContext.Request.Form.Files;
            if (files == null || files.Count == 0)
            {
                ModelState.AddModelError("Files", "Please upload one file");
            }

            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = await _context.Owner.FindAsync(identityUser.OwnerId);

                var service = new ClassifiedListing
                {
                    ClassifiedCategoryId = 3, // Manually set to "Other"
                    ItemName = "Service",
                    Description = description,
                    Price = 0,
                    LastModifiedBy = loggedInUser.FullName,
                    LastModifiedDate = DateTime.Now,
                    ListingDate = DateTime.Now,
                    Owner = loggedInUser,
                    OwnerId = loggedInUser.OwnerId
                };

                _context.ClassifiedListing.Add(service);
                await _context.SaveChangesAsync();

                //image uploading
                string webRootPath = _hostingEnvironment.WebRootPath;

                var uploads = Path.Combine(webRootPath, @"img\ClassifiedsImages");

                var file = files[0];
                var extension = Path.GetExtension(file.FileName);
                using (var filestream = new FileStream(Path.Combine(uploads, service.ClassifiedListingId + extension), FileMode.Create))
                {
                    file.CopyTo(filestream); // moves to server and renames
                }
                var image = new ClassifiedImage()
                {
                    ClassifiedListingId = service.ClassifiedListingId,
                    IsMainImage = (file == files.First()),
                    ImageExtension = extension,
                    ImageURL = @"\" + @"img\ClassifiedsImages" + @"\" + service.ClassifiedListingId + extension
                };

                _context.Add(image);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> EditService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.ClassifiedListing
                .Include(u => u.Images)
                .Include(u => u.Owner)
                .SingleOrDefaultAsync(u => u.ClassifiedListingId == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(int id, string description)
        {
            if (String.IsNullOrEmpty(description))
            {
                ModelState.AddModelError("Description", "Please enter a description");
            }

            var files = HttpContext.Request.Form.Files;

            var service = await _context.ClassifiedListing
                    .Include(u => u.Images)
                    .SingleOrDefaultAsync(u => u.ClassifiedListingId == id);

            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = await _context.Owner.FindAsync(identityUser.OwnerId);
                
                service.Description = description;

                if (files != null && files.Count > 0)
                {
                    var oldImage = service.Images[0];

                    string webRootPath = _hostingEnvironment.WebRootPath;

                    var uploads = Path.Combine(webRootPath, @"img\ClassifiedsImages");

                    var file = files[0];
                    var extension = Path.GetExtension(file.FileName);

                    if (System.IO.File.Exists(Path.Combine(webRootPath, oldImage.ImageURL.Substring(1))))
                    {
                        System.IO.File.Delete(Path.Combine(webRootPath, oldImage.ImageURL.Substring(1)));
                    }

                    using (var filestream = new FileStream(Path.Combine(uploads, service.ClassifiedListingId + extension), FileMode.Create))
                    {
                        file.CopyTo(filestream); // moves to server and renames
                    }

                    oldImage.ImageExtension = extension;
                    oldImage.ImageURL = @"\" + @"img\ClassifiedsImages" + @"\" + service.ClassifiedListingId + extension;
                }
                
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(service);
        }

        // GET: Classifieds/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.ClassifiedListing.SingleOrDefaultAsync(u => u.ClassifiedListingId == id);
            if (item == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _context.Owner.Find(identityUser.OwnerId);

            // Only admins and the owner can delete the classified
            var roles = await _userManager.GetRolesAsync(identityUser);
            if (!roles.Contains("Admin") && !roles.Contains("SuperAdmin") && item.OwnerId != loggedInUser.OwnerId)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Classifieds/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ClassifiedListing listing)
        {
            ClassifiedListing item = await _context.ClassifiedListing.FindAsync(id);

            string webRootPath = _hostingEnvironment.WebRootPath;
            var uploads = Path.Combine(webRootPath, @"img\ClassifiedsImages");
            var oldImages = await _context.ClassifiedImage.Where(x => x.ClassifiedListingId == item.ClassifiedListingId).ToListAsync();
            foreach (var oldImage in oldImages)
            {
                if (System.IO.File.Exists(Path.Combine(webRootPath, oldImage.ImageURL.Substring(1))))
                {
                    System.IO.File.Delete(Path.Combine(webRootPath, oldImage.ImageURL.Substring(1)));
                }
                _context.ClassifiedImage.Remove(oldImage);
            }
            
            item.IsArchive = true;
            _context.ClassifiedListing.Update(item);
            //_context.ClassifiedListing.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}