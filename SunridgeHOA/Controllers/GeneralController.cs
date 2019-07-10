using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunridgeHOA.Models;
using SunridgeHOA.Models.ViewModels;

namespace SunridgeHOA.Controllers
{
    [Authorize(Roles = "Owner")]
    public class GeneralController : Controller
    {
        /*
         * This action was moved to Areas/Owner/OwnerPortal, along with all necessary functionality.
         * 
         * All links to this action have (probably) been moved over to the new action, but I want
         * to keep this action here to redirect to the new one just in case. We can probably remove
         * this eventually.
         * 
         */
        public IActionResult Dashboard()
        {
            return RedirectToAction("Dashboard", "OwnerPortal", new { area = "Owner" });
        }
    }
}