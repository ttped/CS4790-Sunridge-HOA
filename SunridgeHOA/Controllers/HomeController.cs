using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;

namespace SunridgeHOA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public string CurrentFilter { get; set; }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Banner.ToListAsync());
        }

        public IActionResult BoardMembers()
        {
            return View();
        }
        public IActionResult LostFound()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }

        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult Maps()
        {
            return View();
        }

        public IActionResult Forms()
        {
            return View();
        }

        public IActionResult FireInfo()
        {
            return View();
        }

        //public IActionResult News(int year)
        //{
        //    // Change this to pull in news from the db for the given year
        //    // Change to populate a single view rather than one for each year
        //    var viewName = $"News{year}";
        //    return View(viewName);
        //}

        public async Task<IActionResult> News(int year, string searchString)
        {
            var news = _db.NewsItem;
            var newsitem = from item in news
                           where item.Year == year
                           select item;




            

            IQueryable<NewsItem> NewsSearch = from c in _db.NewsItem
                                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                NewsSearch = NewsSearch.Where(c => c.Content.Contains(searchString) || (c.Year.ToString()).Contains(searchString));
            }
            

                


            return View(await NewsSearch.AsNoTracking().ToListAsync());
        }

        //public async Task<IActionResult> News2019()
        //{
        //    var news = _db.NewsItem;
        //    var newsitem = from item in news
        //                   where item.Year == 2019
        //                   select item;

        //    return View(await news.ToListAsync());
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
