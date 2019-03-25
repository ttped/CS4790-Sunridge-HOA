using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;
using SunridgeHOA.Models.ViewModels;

namespace SunridgeHOA.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class ClassifiedsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        [BindProperty]
        public ClassifiedListingViewModel classifiedListingViewModel { get; set; }

        public ClassifiedsController(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            classifiedListingViewModel = new ClassifiedListingViewModel()
            {
                ClassifiedListing = new SunridgeHOA.Models.ClassifiedListing(),
                ClassifiedCategory = _context.ClassifiedCategory.ToList(),
                Owner = _context.Owner.ToList()
            };
        }

        // GET: Classifieds
        public async Task<ActionResult> Index()
        {
            return View(await _context.ClassifiedListing.ToListAsync());
        }

        // GET: Classifieds/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Classifieds/Create
        public ActionResult Create()
        {
            return View(classifiedListingViewModel);
        }

        // POST: Classifieds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassifiedListing listing)
        {
            if (ModelState.IsValid)
            {
                _context.ClassifiedListing.Add(classifiedListingViewModel.ClassifiedListing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classifiedListingViewModel);
        }

        // GET: Classifieds/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.ClassifiedListing.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View();
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ClassifiedListing.Any(e => e.ClassifiedListingId == id))
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
            return View(listing);
        }

        // GET: Classifieds/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Classifieds/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ClassifiedListing listing)
        {
            ClassifiedListing item = await _context.ClassifiedListing.FindAsync(id);

            _context.ClassifiedListing.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}