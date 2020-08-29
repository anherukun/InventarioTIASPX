using InventarioTIASPX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class ComputersController : Controller
    {
        // GET: Computers
        public ActionResult Index()
        {
            ViewData["computers"] = RepositoryComputer.GetAllComputers();
            return View();
        }

        public ActionResult NewComputer()
        {
            ViewData["processors"] = RepositoryDevice.GetAllProcessors(false);
            return View();
        }
    }
}