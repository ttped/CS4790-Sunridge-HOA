using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SunridgeHOA.Areas.Owner.Controllers
{
    //Add/Edit/Delete (Disable) Photos (REPEAT OF ADMIN, BUT FILTERED BY OWNER)
    [Area("Owner")]
    public class PhotosController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Add()
        //{
        //    return View();
        //}

        public IActionResult Edit()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Edit()
        //{
        //    return View();
        //}

        public IActionResult Delete()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Delete()
        //{
        //    return View();
        //}

    }
}