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
using SunridgeHOA.Models;
using SunridgeHOA.Utility;

namespace SunridgeHOA.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhotosController(ApplicationDbContext db, HostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            if (identityUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);

            // Redirect to /MyPhotos if the user is not an admin
            if (!roles.Contains("Admin") && !roles.Contains("SuperAdmin"))
            {
                return RedirectToAction(nameof(MyPhotos));
            }

            var photos = _db.Photo.Include(m => m.Owner);
            return View(await photos.ToListAsync());
        }

        public async Task<IActionResult> MyPhotos()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _db.Owner.Find(identityUser.OwnerId);

            var ownerPhotos = await _db.Photo
                .Include(m => m.Owner)
                .Where(m => m.OwnerId == loggedInUser.OwnerId)
                .ToListAsync();
            return View(ownerPhotos);
        }

        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(new string[] { "Summer", "Winter", "People" });
            return View();
        }

        //POST: AdminPhoto Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST([Bind("Image, Title, Category, Year")] Photo photo)
        {
            if (photo.Category == "-1")
            {
                ModelState.AddModelError("Photo.Category", "Please select a category");
            }

            var files = HttpContext.Request.Form.Files;
            if (files.Count == 0)
            {
                ModelState.AddModelError("Photo.Image", "Please select an image");
            }

            if (photo.Year < 2000 || photo.Year > DateTime.Now.Year)
            {
                ModelState.AddModelError("Photo.Year", $"The year must be between 2000 and {DateTime.Now.Year}");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Category"] = new SelectList(new string[] { "Summer", "Winter", "People" });
                return View();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            if (identityUser == null)
            {
                return RedirectToPage(nameof(MyPhotos));
            }

            var loggedInUser = _db.Owner.Find(identityUser.OwnerId);
            photo.OwnerId = loggedInUser.OwnerId;

            photo.Year = DateTime.Now.Year;

            _db.Photo.Add(photo);
            await _db.SaveChangesAsync();

            //Save Physical Image
            string webRootPath = _hostingEnvironment.WebRootPath;

            var photosFromDb = _db.Photo.Find(photo.PhotoId);

            if (files.Count != 0)
            {
                //Image(s) has been uploaded with form
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, photo.PhotoId + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream); //moves to server and renames
                }

                //now I know the new image name, so I can save the STRING image to the database
                photosFromDb.Image = @"\" + SD.ImageFolder + @"\" + photo.PhotoId + extension;
            }
            else
            {
                //when the user didn't give us an image so we'll upload the placeholder
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + photo.PhotoId + ".jpg");
                photosFromDb.Image = @"\" + SD.ImageFolder + @"\" + photo.PhotoId + ".jpg";
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(MyPhotos));
        }

        //GET: AdminPhoto Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var loggedInUser = _db.Owner.Find(identityUser.OwnerId);
            if (identityUser == null)
            {
                return RedirectToAction("MyPhotos");
            }

            var photo = await _db.Photo
                    .Include(m => m.Owner)
                    .FirstOrDefaultAsync(m => m.PhotoId == id);

            photo.OwnerId = loggedInUser.OwnerId;

            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        //GET: AdminPhoto Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _db.Photo
                .Include(m => m.Owner)
                .SingleOrDefaultAsync(m => m.PhotoId == id);

            if (photo == null)
            {
                return NotFound();
            }

            ViewData["Category"] = new SelectList(new string[] { "Summer", "Winter", "People" });
            return View(photo);
        }

        //POST: AdminPhoto Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Image, Title, Year, Category")] Photo photo)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                var loggedInUser = _db.Owner.Find(identityUser.OwnerId);
                var photoFromDb = _db.Photo.Where(m => m.PhotoId == id).FirstOrDefault();

                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0 && files[0] != null)
                {
                    //if user uploads a new image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(photoFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, photo.PhotoId + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, photo.PhotoId + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, photo.PhotoId + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    photo.Image = @"\" + SD.ImageFolder + @"\" + photo.PhotoId + extension_new;
                }

                if (photo.Image != null)
                {
                    photoFromDb.Image = photo.Image;
                }

                photoFromDb.Image = photo.Image;
                photoFromDb.Title = photo.Title;
                photoFromDb.Year = photo.Year;
                photoFromDb.Category = photo.Category;

                await _db.SaveChangesAsync();

                return RedirectToAction("MyPhotos");
            }

            ViewData["Category"] = new SelectList(new string[] { "Summer", "Winter", "People" });
            return View(photo);
        }

        //GET: AdminPhoto Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _db.Photo
                .Include(m => m.Owner)
                .FirstOrDefaultAsync(m => m.PhotoId == id);

            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);

        }

        //POST: AdminPhoto Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _db.Photo.FindAsync(id);

            string webRootPath = _hostingEnvironment.WebRootPath;
            var uploads = Path.Combine(webRootPath, SD.ImageFolder);

            if (System.IO.File.Exists(Path.Combine(webRootPath, photo.Image)))
            {
                System.IO.File.Delete(Path.Combine(webRootPath, photo.Image));
            }

            _db.Photo.Remove(photo);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(MyPhotos));
        }
    }
}