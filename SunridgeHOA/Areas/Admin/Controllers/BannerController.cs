using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public BannerController(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Banner
        public async Task<IActionResult> Index()
        {
            return View(await _context.Banner.ToListAsync());
        }

        // GET: Banner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            var banner = await _context.Banner.SingleOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Banner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner)
        {
            if (ModelState.IsValid)
            {
                //banner.Image = "hi";
                _context.Banner.Add(banner);
                await _context.SaveChangesAsync();

                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productsFromDb = _context.Banner.Find(banner.Id);
                productsFromDb.Image = @"\" + @"img\BannerImages" + @"\" + banner.Id + ".jpg";
                if (files.Count != 0)
                {
                    var uploads = Path.Combine(webRootPath, @"img\BannerImages");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(uploads, banner.Id + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream); // moves to server and renames
                    }

                    banner.Image = @"\" + @"img\BannerImages" + @"\" + banner.Id + extension;
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Banner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Banner.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Banner banner)
        {
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Banner.Any(e => e.Id == id))
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
            return View(banner);
        }

        // GET: Banner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Banner/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Banner banner)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Banner item = await _context.Banner.FindAsync(id);

            var uploads = Path.Combine(webRootPath, @"img\BannerImages");
            var extension = Path.GetExtension(item.Image);
            if (System.IO.File.Exists(Path.Combine(uploads, item.Id + extension)))
            {
                System.IO.File.Delete(Path.Combine(uploads, item.Id + extension));
            }

            _context.Banner.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}