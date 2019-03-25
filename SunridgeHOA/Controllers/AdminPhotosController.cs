using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;
using SunridgeHOA.Models.ViewModels;
using SunridgeHOA.Utility;

namespace SunridgeHOA.Controllers
{
    //[Authorize(Roles = SD.AdminEndUser)]
    public class AdminPhotosController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        [BindProperty]
        public AdminPhotoViewModels AdminPhotoVM { get; set; }

        public AdminPhotosController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            /*
            AdminPhotoVM = new AdminPhotoViewModels()
            {
                //Owner = _db.Owner.ToList(),
                Photo = new Models.Photo()
            };
            */
        }

        //GET: AdminPhoto Index
        public async Task<IActionResult> Index()
        {
            //var photos = _db.Photo.Include(m => m.Owner);
            var photos = _db.Photo;
            return View(await photos.ToListAsync());
        }

        //GET: AdminPhoto Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(new string[] { "Summer", "Winter", "People" });
            return View(AdminPhotoVM);
            //return View();
        }

        //POST: AdminPhoto Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return View(AdminPhotoVM);
                //return View();
            }

            _db.Photo.Add(AdminPhotoVM.Photo);
            await _db.SaveChangesAsync();


            //Save Physical Image
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var photosFromDb = _db.Photo.Find(AdminPhotoVM.Photo.PhotoId);

            if (files.Count != 0)
            {
                //Image(s) has been uploaded with form
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, AdminPhotoVM.Photo.PhotoId + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream); //moves to server and renames
                }

                //now I know the new image name, so I can save the STRING image to the database
                photosFromDb.Image = @"\" + SD.ImageFolder + @"\" + AdminPhotoVM.Photo.PhotoId + extension;
            }
            else
            {
                //when the user didn't give us an image so we'll upload the placeholder
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + AdminPhotoVM.Photo.PhotoId + ".jpg");
                photosFromDb.Image = @"\" + SD.ImageFolder + @"\" + AdminPhotoVM.Photo.PhotoId + ".jpg";
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: AdminPhoto Details
        public async Task<IActionResult> Details(int? id)
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

            //return View();
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

            //return View();
        }

        //POST: AdminPhoto Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Image, Title, Year, Category")] Models.Photo photo, Owner owner)
        {
            if (id != photo.PhotoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var photoFromDb = _db.Photo.Where(m => m.PhotoId == photo.PhotoId).FirstOrDefault();

                photoFromDb.Image = photo.Image;
                photoFromDb.Title = photo.Title;
                photoFromDb.Year = photo.Year;
                photoFromDb.Category = photo.Category;
                photoFromDb.OwnerId = photo.OwnerId;

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            //return View();
        }

        //POST: AdminPhoto Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var photo = await _db.Photo
            //    .Include(m => m.Owner)
            //    .FirstOrDefaultAsync(m => m.PhotoId == id);

            var photo = await _db.Photo.FindAsync(id);

            if (photo == null)
            {
                return NotFound();
            }
            else
            {
                _db.Photo.Remove(photo);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

    }
}