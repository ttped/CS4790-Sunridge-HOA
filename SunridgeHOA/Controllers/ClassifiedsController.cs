using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SunridgeHOA.Controllers
{
    public class ClassifiedsController : Controller
    {
        public IActionResult Lots()
        {
            return View();
        }

        public IActionResult Cabins()
        {
            return View();
        }

        public IActionResult Other()
        {
            return View();
        }
    }
}