using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Controllers
{
    public class ReportsController : Controller
    {
        [HttpGet]
        public IActionResult OrderReceipt()
        {
            return View();
        }
    }
}
