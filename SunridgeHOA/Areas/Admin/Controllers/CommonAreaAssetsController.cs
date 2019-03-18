using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;

namespace SunridgeHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonAreaAssetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommonAreaAssetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CommonAreaAssets
        public async Task<IActionResult> Index()
        {
            return View(await _context.CommonAreaAsset.ToListAsync());
        }

        // GET: Admin/CommonAreaAssets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commonAreaAsset = await _context.CommonAreaAsset
                .FirstOrDefaultAsync(m => m.CommonAreaAssetId == id);
            if (commonAreaAsset == null)
            {
                return NotFound();
            }

            return View(commonAreaAsset);
        }

        // GET: Admin/CommonAreaAssets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CommonAreaAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommonAreaAssetId,AssetName,PurchasePrice,Description,Status,Date,IsArchive,LastModifiedBy,LastModifiedDate")] CommonAreaAsset commonAreaAsset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commonAreaAsset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commonAreaAsset);
        }

        // GET: Admin/CommonAreaAssets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commonAreaAsset = await _context.CommonAreaAsset.FindAsync(id);
            if (commonAreaAsset == null)
            {
                return NotFound();
            }
            return View(commonAreaAsset);
        }

        // POST: Admin/CommonAreaAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommonAreaAssetId,AssetName,PurchasePrice,Description,Status,Date,IsArchive,LastModifiedBy,LastModifiedDate")] CommonAreaAsset commonAreaAsset)
        {
            if (id != commonAreaAsset.CommonAreaAssetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commonAreaAsset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommonAreaAssetExists(commonAreaAsset.CommonAreaAssetId))
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
            return View(commonAreaAsset);
        }

        // GET: Admin/CommonAreaAssets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commonAreaAsset = await _context.CommonAreaAsset
                .FirstOrDefaultAsync(m => m.CommonAreaAssetId == id);
            if (commonAreaAsset == null)
            {
                return NotFound();
            }

            return View(commonAreaAsset);
        }

        // POST: Admin/CommonAreaAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commonAreaAsset = await _context.CommonAreaAsset.FindAsync(id);
            _context.CommonAreaAsset.Remove(commonAreaAsset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommonAreaAssetExists(int id)
        {
            return _context.CommonAreaAsset.Any(e => e.CommonAreaAssetId == id);
        }
    }
}
