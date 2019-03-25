using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SunridgeHOA.Controllers
{
    public class GeneralController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult MyLots()
        {
            return View();
        }

        public IActionResult BillingHistory()
        {
            return View();
        }

        //public IActionResult Classifieds()
        //{
        //    return View();
        //}

        public IActionResult ViewAds()
        {
            return View();
        }

        public IActionResult PostNewAd()
        {
            return View();
        }
    }
}