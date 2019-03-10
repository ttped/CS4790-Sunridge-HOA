using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SunridgeHOA.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class OwnerPortalController : Controller
    {
        //Create a Login (with automatic password retrieval in case they forget)
        public IActionResult Login()
        {
            return View();
        }

        //View Lot Information, History (Claims, Payments, Complaints, Documents, etc.)
        public IActionResult Lot()
        {
            return View();
        }

        //Edit Limited (Personal) Owner Information
        public IActionResult OwnerInfo()
        {
            return View();
        }

        //Make a Payment (link to Paypal)
        public IActionResult Payment()
        {
            return View();
        }

        //File a Claim, Complaint, Exception to the Rule, or “Work in Kind” Form
        //View the history and response of the item above
        public IActionResult File()
        {
            return View();
        }

    }
}