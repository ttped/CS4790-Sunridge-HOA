using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SunridgeHOA.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult CommonAreaAssets()
        {
            return View();
        }

        public IActionResult Keys()
        {
            return View();
        }

        public IActionResult OwnerActivity()
        {
            return View();
        }

        public IActionResult Owners()
        {
            return View();
        }

        public IActionResult Transactions()
        {
            return View();
        }
    }
}