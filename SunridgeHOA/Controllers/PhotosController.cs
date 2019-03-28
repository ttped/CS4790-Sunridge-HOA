using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;

namespace SunridgeHOA.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PhotosController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // possibly load the different types of categories for photos if we are expanding the category selection
            return View();
        }

        /*
         * These could be removed in favor of one function that loads pictures by category dynamically using route parameters.
         * This would make the route ugly, but we can fix that with a new route in Startup.cs specifically for this case
         */
        public async Task<IActionResult> Summer()
        {
            //var summerPhoto = _db.Photo.Include(m => m.Owner);
            var summerPhoto = _db.Photo;
            return View(await summerPhoto.ToListAsync());
        }

        public async Task<IActionResult> Winter()
        {
            //var winterPhoto = _db.Photo.Include(m => m.Owner);
            var winterPhoto = _db.Photo;
            return View(await winterPhoto.ToListAsync());
        }

        public async Task<IActionResult> People()
        {
            //var peoplePhoto = _db.Photo.Include(m => m.Owner);
            var peoplePhoto = _db.Photo;
            return View(await peoplePhoto.ToListAsync());
        }

    }
}