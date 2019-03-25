using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;

namespace SunridgeHOA.Controllers
{
    public class NewsItemController : Controller
    {
        public readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        public string ImageContentType { get; set; }
        public byte[] Image { get; set; }

        public NewsItemController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        //GET: Index NewsItem
        public IActionResult Index()
        {
            return View(_db.NewsItem.ToList());
        }

        //GET: Create NewsItem
        public IActionResult Create()
        {
            ViewData["Year"] = new SelectList(new string[] { "2019", "2018", "2017", "2016" });
            return View();
        }

        //POST: Create NewsItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Header, Content, Year")] Models.NewsItem newsItem, File file)
        {
            if (ModelState.IsValid)
            {
                /*
                //Save Physical Image
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productsFromDb = _db.Products.Find(ProductsVM.Products.Id);

                if (files.Count != 0)
                {
                    //Image(s) has been uploaded with form
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream); //moves to server and renames
                    }

                    //now I know the new image name, so I can save the STRING image to the database
                    productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension;
                }
                else
                {
                    //when the user didn't give us an image so we'll upload the placeholder
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                    System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg");
                    productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg";
                }
                */

                newsItem.File = file;
                _db.Add(newsItem);
                //_db.NewsItem.Add(newsItem);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Year"] = new SelectList(new string[] { "2019", "2018", "2017", "2016" });
            return View(newsItem);
        }

        //GET: NewsItem Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _db.NewsItem
                //.Include(m => m.File)
                .FirstOrDefaultAsync(m => m.NewsItemId == id);

            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        //GET: Edit NewsItem
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var news = await _db.NewsItem
            //    .Include(m => m.File)
            //    .SingleOrDefaultAsync(m => m.NewsItemId == id);

            //if (news == null)
            //{
            //    return NotFound();
            //}

            //ViewData["Year"] = new SelectList(new string[] { "2019", "2018", "2017", "2016" });
            //return View(news);

            return View();
        }

        //POST: Edit NewsItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Header, Content, Year")] Models.NewsItem newsItem, File file)
        {
            if (id != newsItem.NewsItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var newsFromDb = _db.NewsItem.Where(m => m.NewsItemId == newsItem.NewsItemId).FirstOrDefault();

                newsFromDb.Header = newsItem.Header;
                newsFromDb.Content = newsItem.Content;
                newsFromDb.Year = newsItem.Year;
                newsFromDb.FileId = file.FileId;

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Year"] = new SelectList(new string[] { "2019", "2018", "2017", "2016" });
            return View(newsItem);
        }

        //GET: Delete NewsItem
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _db.NewsItem
                .Include(m => m.File)
                .FirstOrDefaultAsync(m => m.NewsItemId == id);

            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        //POST: Delete NewsItem
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var news = await _db.NewsItem
            //    .Include(m => m.File)
            //    .FirstOrDefaultAsync(m => m.NewsItemId == id);

            var news = await _db.NewsItem.FindAsync(id);
            
            if (news == null)
            {
                return NotFound();
            }
            else
            {
                _db.NewsItem.Remove(news);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}