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

            var classifieds = await _context.ClassifiedListing
                .Include(u => u.Images)
                .Include(u => u.ClassifiedCategory)
                .Where(u => u.ClassifiedCategoryId == 3)
                .Where(u => u.Images.Any())
                .Select(u => u.Images[0].ImageURL)
                .ToListAsync();
            return View(classifieds);
        }
    }
}