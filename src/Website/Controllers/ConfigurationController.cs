using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Controllers
{
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public IActionResult _Company()
        {
            return PartialView();
        }
    }
}
