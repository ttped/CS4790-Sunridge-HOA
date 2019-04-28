using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Areas.Owner.Models.ViewModels;
using SunridgeHOA.Models;

namespace SunridgeHOA.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class FormsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HostingEnvironment _hostingEnv;

        public FormsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, HostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnv = env;
        }

        public IActionResult Index()
        {
            var forms = _context.FormResponse.Include(u => u.Owner).ToList();
            var vm = new FormIndexVM();
            foreach (var form in forms)
            {
                if (form.Resolved)
                {
                    vm.Resolved.Add(form);
                }
                else
                {
                    vm.Unresolved.Add(form);
                }
            }

            return View(vm);
        }

        public async Task<IActionResult> MyForms()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var owner = _context.Owner.Find(user.OwnerId);

            var forms = await _context.FormResponse
                .Include(u => u.Owner)
                .Where(u => u.OwnerId == owner.OwnerId)
                .ToListAsync();
            var vm = new FormIndexVM();
            foreach (var form in forms)
            {
                if (form.Resolved)
                {
                    vm.Resolved.Add(form);
                }
                else
                {
                    vm.Unresolved.Add(form);
                }
            }

            return View(vm);
        }

        public async Task<IActionResult> SuggestionComplaint(int? id)
        {
            if (id == null)
            {
                return View(new FormResponse { FormResponseId = 0 });
            }

            var form = await _context.FormResponse.Include(u => u.Owner).SingleOrDefaultAsync(u => u.FormResponseId == id);
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> SuggestionComplaint(FormResponse form)
        {
            if (String.IsNullOrEmpty(form.Description))
            {
                ModelState.AddModelError("Description", "Please fill in the description field");
            }
            if (String.IsNullOrEmpty(form.Suggestion))
            {
                ModelState.AddModelError("Suggestion", "Please fill in the suggestion field");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var owner = _context.Owner.Find(user.OwnerId);

                var existingForm = await _context.FormResponse.SingleOrDefaultAsync(u => u.FormResponseId == form.FormResponseId);
                if (existingForm == null)
                {
                    var newForm = new FormResponse
                    {
                        FormType = "SC",
                        OwnerId = owner.OwnerId,
                        SubmitDate = DateTime.Now,
                        Description = form.Description,
                        Suggestion = form.Suggestion,
                        PrivacyLevel = "Public"
                    };

                    _context.Add(newForm);
                }
                else
                {
                    existingForm.Description = form.Description;
                    existingForm.Suggestion = form.Suggestion;
                }
                
                await _context.SaveChangesAsync();

                return RedirectToAction("Dashboard", "General", new { area = "" });
            }

            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Resolve(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var owner = _context.Owner.Find(user.OwnerId);

            var form = await _context.FormResponse.FindAsync(id);
            form.Resolved = true;
            form.ResolveDate = DateTime.Now;
            form.ResolveUser = owner.FullName;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}