using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Controllers
{
    public class ServicesController : Controller
    {
        [HttpGet]
        public IActionResult _Index()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult _ServiceUI()
        {
            return PartialView();
        }
    }
}
