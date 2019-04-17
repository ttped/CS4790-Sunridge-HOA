using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;
using SunridgeHOA.Utility;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class NewsItemController : Controller
    {
        public readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

       //public NewsItem NewsItem { get; set; }
        public NewsItemController(ApplicationDbContext db, HostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        //GET: Index NewsItem
        public async Task<IActionResult> Index()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            //var loggedInUser = _context.Owner.Find(identityUser.OwnerId);
            if (identityUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //var news = _db.NewsItem.Include(m => m.File);
            var news = _db.NewsItem;
            return View(await _db.NewsItem.ToListAsync());
        }

        //GET: Create NewsItem
        public IActionResult Create()
        {
            ViewData["Year"] = new SelectList(new string[] { "Select an year", "2019", "2018", "2017", "2016" });
            return View();
        }

        //POST: Create NewsItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Header, Content, Year")] Models.NewsItem newsItem)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            if (identityUser == null)
            {
                return RedirectToPage(nameof(Index));
            }

            //var news = _db.File.Find();
            //newsItem.FileId = news.FileId;

            //if (ModelState.IsValid)
            {
                //newsItem.File = file;
                _db.NewsItem.Add(newsItem);
                await _db.SaveChangesAsync();

                //Save Physical Image------------------------------------------------------------------------------------
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                //var photosFromDb = _db.Photo.Find(AdminPhotoVM.Photo.PhotoId);
                var newsFromDb = _db.NewsItem.Find(newsItem.NewsItemId);

                if (files.Count != 0)
                {
                    //Image(s) has been uploaded with form
                    var uploads = Path.Combine(webRootPath, SD.NewsImageFolder);
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(uploads, newsItem.NewsItemId + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream); //moves to server and renames
                    }

                    //now I know the new image name, so I can save the STRING image to the database
                    newsFromDb.Image = @"\" + SD.NewsImageFolder + @"\" + newsItem.NewsItemId + extension;
                }
                else
                {
                    //when the user didn't give us an image so we'll upload the placeholder
                    var uploads = Path.Combine(webRootPath, SD.NewsImageFolder + @"\" + SD.DefaultImage);
                    System.IO.File.Copy(uploads, webRootPath + @"\" + SD.NewsImageFolder + @"\" + newsItem.NewsItemId + ".jpg");
                    newsFromDb.Image = @"\" + SD.NewsImageFolder + @"\" + newsItem.NewsItemId + ".jpg";
                }
                //------------------------------------------------------------------------------------------------------

                //newsItem.File = file;
                //_db.Add(newsItem);
                //_db.NewsItem.Add(newsItem);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //ViewData["Year"] = new SelectList(new string[] { "Select an year", "2019", "2018", "2017", "2016" });
            //return View(newsItem);
        }

        //GET: NewsItem Details
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
                return RedirectToAction("Index");
            }

            var news = await _db.NewsItem
                //.Include(m => m.File)
                .FirstOrDefaultAsync(m => m.NewsItemId == id);

            //news.FileId = .FileId;

            if (news == null)
            {
                return NotFound();
            }
            return View(news);

            //return View();
        }

        //GET: Edit NewsItem
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _db.NewsItem
                //.Include(m => m.File)
                .SingleOrDefaultAsync(m => m.NewsItemId == id);

            if (news == null)
            {
                return NotFound();
            }

            ViewData["Year"] = new SelectList(new string[] { "2019", "2018", "2017", "2016" });
            return View(news);

            //return View();
        }

        //POST: Edit NewsItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Header, Content, Year")] Models.NewsItem newsItem)
        {
            //if (id != newsItem.NewsItemId)
            //{
            //    return NotFound();
            //}

            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var newsFromDb = _db.NewsItem.Where(m => m.NewsItemId == id).FirstOrDefault();

                if (files.Count > 0 && files[0] != null)
                {
                    //if user uploads a new image
                    var uploads = Path.Combine(webRootPath, SD.NewsImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(newsFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, newsItem.NewsItemId + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, newsItem.NewsItemId + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, newsItem.NewsItemId + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    newsItem.Image = @"\" + SD.NewsImageFolder + @"\" + newsItem.NewsItemId + extension_new;
                }

                if (newsItem.Image != null)
                {
                    newsFromDb.Image = newsItem.Image;
                }

                newsFromDb.Header = newsItem.Header;
                newsFromDb.Content = newsItem.Content;
                newsFromDb.Year = newsItem.Year;
                newsFromDb.Image = newsItem.Image;
                //newsFromDb.FileId = file.FileId;

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
                //.Include(m => m.File)
                .FirstOrDefaultAsync(m => m.NewsItemId == id);

            if (news == null)
            {
                return NotFound();
            }
            return View(news);

            //return View();
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