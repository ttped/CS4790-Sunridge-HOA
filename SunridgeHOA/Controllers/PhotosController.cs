using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SunridgeHOA.Controllers
{
    public class PhotosController : Controller
    {
        public IActionResult Index()
        {
            // possibly load the different types of categories for photos if we are expanding the category selection
            return View();
        }


        /*
         * These could be removed in favor of one function that loads pictures by category dynamically using route parameters.
         * This would make the route ugly, but we can fix that with a new route in Startup.cs specifically for this case
         */
        public IActionResult Summer()
        {
            return View();
        }

        public IActionResult Winter()
        {
            return View();
        }

        public IActionResult People()
        {
            return View();
        }
    }
}