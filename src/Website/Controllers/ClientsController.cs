using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class ClientsController : Controller
    {
        [HttpGet]
        public IActionResult _Index()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult _ClientUI()
        {
            return PartialView();
        }
    }
}
