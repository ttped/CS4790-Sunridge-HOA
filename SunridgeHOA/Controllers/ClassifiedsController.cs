using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;
using SunridgeHOA.Models.ViewModels;

namespace SunridgeHOA.Controllers
{
    public class ClassifiedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassifiedsController(ApplicationDbContext context)
        {
            _context = context;
        }



        public async  Task<IActionResult> Lots()
        {
            var classifieds = new List<ClassifiedListing>();
            var items = await _context.ClassifiedListing.ToListAsync();
            var lotCategory = await _context.ClassifiedCategory.Where(x => x.Description == "Lots").ToListAsync();
            //lotCategory = await lotCategory.Where(x => x.Description == "Lots").ToListAsync();
            foreach (var item in items)
            {
                item.Images = await _context.ClassifiedImage.Where(x => x.ClassifiedListingId == item.ClassifiedListingId).ToListAsync();
                item.ClassifiedCategory = await _context.ClassifiedCategory.Where(x => x.ClassifiedCategoryId == item.ClassifiedCategoryId).FirstAsync();
                classifieds.Add(item);
            }
            return View(classifieds);
        }

        public async Task<IActionResult> Cabins()
        {
            //var classifiedsVM = new List<ClassifiedListingPageViewModel>();
            var classifieds = new List<ClassifiedListing>();
            var items = await _context.ClassifiedListing.ToListAsync();
            foreach(var item in items)
            {
                item.Images = await _context.ClassifiedImage.Where(x => x.ClassifiedListingId == item.ClassifiedListingId).ToListAsync();
                item.ClassifiedCategory = await _context.ClassifiedCategory.Where(x => x.ClassifiedCategoryId == item.ClassifiedCategoryId).FirstAsync() ;
                classifieds.Add(item);
            }
            return View(classifieds);
        }

        public async Task<IActionResult> Other()
        {
            //var classifieds = new List<ClassifiedListing>();
            //var items = await _context.ClassifiedListing.ToListAsync();
            //foreach (var item in items)
            //{
            //    item.Images = await _context.ClassifiedImage.Where(x => x.ClassifiedListingId == item.ClassifiedListingId).ToListAsync();
            //    item.ClassifiedCategory = await _context.ClassifiedCategory.Where(x => x.ClassifiedCategoryId == item.ClassifiedCategoryId).FirstAsync();
            //    classifieds.Add(item);
            //}
            //return View(classifieds);

            var otherCategory = await _context.ClassifiedCategory
                .SingleAsync(u => u.Description == "Other");
            var classifieds = await _context.ClassifiedListing
                .Include(u => u.Images)
                .Include(u => u.ClassifiedCategory)
                .Where(u => u.ClassifiedCategoryId == otherCategory.ClassifiedCategoryId)
                .Where(u => u.Images.Any())
                .Where(u => !u.IsArchive)
                .Select(u => u.Images[0].ImageURL)
                .ToListAsync();
            return View(classifieds);
        }

        public async Task<IActionResult> ATVs()
        {
            var atvCategory = await _context.ClassifiedCategory
                .SingleAsync(u => u.Description == "ATVs");
            var classifieds = await _context.ClassifiedListing
                .Include(u => u.Images)
                .Where(u => u.ClassifiedCategoryId == atvCategory.ClassifiedCategoryId)
                .Where(u => !u.IsArchive)
                .ToListAsync();
            return View(classifieds);
        }

        public async Task<IActionResult> Trailers()
        {
            var trailerCategory = await _context.ClassifiedCategory
                .SingleAsync(u => u.Description == "Trailers");
            var classifieds = await _context.ClassifiedListing
                .Include(u => u.Images)
                .Where(u => u.ClassifiedCategoryId == trailerCategory.ClassifiedCategoryId)
                .Where(u => !u.IsArchive)
                .ToListAsync();
            return View(classifieds);
        }
    }
}