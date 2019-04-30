using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class ServiceOrdersController : Controller
    {
        public IActionResult _Index()
        {
            return PartialView();
        }

        public IActionResult _OrderServiceUI()
        {
            return PartialView();
        }

        public IActionResult _OrderServiceItemUI()
        {
            return PartialView();
        }
    }
}