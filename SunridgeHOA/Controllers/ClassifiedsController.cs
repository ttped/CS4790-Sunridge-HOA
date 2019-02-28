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
            //TODO pull all lots from the classifieds table from the db and send them to the view
            return View();
        }

        public IActionResult Cabins()
        {
            //TODO pull all cabins from the classifieds table from the db and send them to the view
            return View();
        }

        public IActionResult Other()
        {
            //TODO pull all other services from the classifieds table from the db and send them to the view
            return View();
        }
    }
}