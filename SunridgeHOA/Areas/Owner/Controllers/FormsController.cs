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

        [Authorize(Roles = "SuperAdmin, Admin")]
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

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var owner = _context.Owner.Find(user.OwnerId);
            
            var form = await _context.FormResponse
                .Include(u => u.Owner)
                .Include(u => u.Comments).ThenInclude(u => u.Owner)
                .SingleOrDefaultAsync(u => u.FormResponseId == id);
           
            if (!roles.Contains("Admin") && !roles.Contains("SuperAdmin"))
            {
                if (form.OwnerId != owner.OwnerId)
                {
                    return NotFound();
                }
            }

            //var comments = await _context.Comment
            //    .Include(u => u.Owner)
            //    .Where(u => u.FormResponseId == form.FormResponseId)
            //    .OrderBy(u => u.Date)
            //    .ToListAsync();
            //form.Comments = comments;

            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                //return RedirectToAction("Dashboard", "General", new { area = "" });
                return RedirectToAction("Dashboard", "OwnerPortal", new { area = "Owner" });
            }

            return View(form);
        }

        public async Task<IActionResult> InKindWork(int? id)
        {
            if (id == null)
            {
                return View(new InKindWorkSubmission());
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var owner = _context.Owner.Find(user.OwnerId);

            var form = await _context.FormResponse
                .Include(u => u.Owner)
                .Include(u => u.InKindWorkHours)
                .Include(u => u.Comments).ThenInclude(u => u.Owner)
                .SingleOrDefaultAsync(u => u.FormResponseId == id);

            if (!roles.Contains("Admin") && !roles.Contains("SuperAdmin"))
            {
                if (form.OwnerId != owner.OwnerId)
                {
                    return NotFound();
                }
            }

            return View(new InKindWorkSubmission
            {
                FormResponse = form,
                LaborHours = form.InKindWorkHours.Where(u => u.Type == "Labor").ToList(),
                EquipmentHours = form.InKindWorkHours.Where(u => u.Type == "Equipment").ToList()
            });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InKindWork(InKindWorkSubmission form)
        {
            if (String.IsNullOrEmpty(form.FormResponse.Description))
            {
                ModelState.AddModelError("FormResponse.Description", "Please fill in the \"Describe Activity\" field");
            }
            if (String.IsNullOrEmpty(form.FormResponse.Suggestion))
            {
                ModelState.AddModelError("FormResponse.Suggestion", "Please fill in the \"Describe Equipment\" field");
            }

            // Check hour validation here
            var hourEntries = new List<InKindWorkHours>();
            foreach (var entry in form.LaborHours)
            {
                var hasActivity = !String.IsNullOrEmpty(entry.Description);
                var hasHours = entry.Hours != null && entry.Hours != 0;
                if ((hasActivity && !hasHours) || (!hasActivity && hasHours))
                {
                    ModelState.AddModelError("LaborHours", "Please make sure all labor activities have a filled in hours value");
                }
                else if (hasActivity && hasHours)
                {
                    entry.Type = "Labor";
                    hourEntries.Add(entry);
                }
            }

            foreach (var entry in form.EquipmentHours)
            {
                var hasActivity = !String.IsNullOrEmpty(entry.Description);
                var hasHours = entry.Hours != null && entry.Hours != 0;
                if ((hasActivity && !hasHours) || (!hasActivity && hasHours))
                {
                    ModelState.AddModelError("EquipmentHours", "Please make sure all equipment entries have a filled in hours value");
                }
                else if (hasActivity && hasHours)
                {
                    entry.Type = "Equipment";
                    hourEntries.Add(entry);
                }
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var owner = _context.Owner.Find(user.OwnerId);

                var existingForm = await _context.FormResponse.SingleOrDefaultAsync(u => u.FormResponseId == form.FormResponse.FormResponseId);
                if (existingForm == null)
                {
                    var newForm = new FormResponse
                    {
                        FormType = "WIK",
                        OwnerId = owner.OwnerId,
                        SubmitDate = DateTime.Now,
                        Description = form.FormResponse.Description,
                        Suggestion = form.FormResponse.Suggestion,
                        PrivacyLevel = "Public",
                        InKindWorkHours = hourEntries
                    };

                    _context.Add(newForm);
                }
                else
                {
                    existingForm.Description = form.FormResponse.Description;
                    existingForm.Suggestion = form.FormResponse.Suggestion;
                    existingForm.InKindWorkHours = hourEntries;
                }

                await _context.SaveChangesAsync();

                //return RedirectToAction("Dashboard", "General", new { area = "" });
                return RedirectToAction("Dashboard", "OwnerPortal", new { area = "Owner" });
            }

            return View(form);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> Resolve(int id, string resolution)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var owner = _context.Owner.Find(user.OwnerId);

            var comment = new Comment
            {
                OwnerId = owner.OwnerId,
                Content = resolution,
                Date = DateTime.Now,
                FormResponseId = id
            };

            _context.Add(comment);

            var form = await _context.FormResponse.FindAsync(id);
            form.Resolved = true;
            form.ResolveDate = DateTime.Now;
            form.ResolveUser = owner.FullName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}